using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using ServerlessCommunity.Application.Command.Collect;
using ServerlessCommunity.Application.Command.Render;
using ServerlessCommunity.Application.Data;
using ServerlessCommunity.Application.Data.Model;
using ServerlessCommunity.Application.ViewModel.Meetup;
using ServerlessCommunity.Config;
using ServerlessCommunity.AzFunc._Extensions;
using ServerlessCommunity.Data.AzStorage.Table;
using ServerlessCommunity.Data.AzStorage.Table.Model;

namespace ServerlessCommunity.AzFunc.Collector
{
    public static class MeetupPageDataCollector
    {
        [FunctionName(nameof(MeetupPageDataCollector))]
        public static async Task CollectMeetupPageDataFunction(
            [QueueTrigger(QueueName.CollectMeetupPage)]CollectMeetupPageData command,
            
            [Table(TableName.Meetup)] CloudTable meetupTable,
            [Table(TableName.MeetupSession)] CloudTable meetupSessionTable,
            [Table(TableName.Session)] CloudTable sessionTable,
            [Table(TableName.Speaker)] CloudTable speakerTable,
            [Table(TableName.Venue)] CloudTable venueTable,
            
            [Blob(ContainerName.PageData + "/meetup-{" + nameof(CollectMeetupPageData.MeetupId) + "}.json", FileAccess.Write)]CloudBlockBlob meetupPageDataBlob,
            [Queue(QueueName.RenderPage)]CloudQueue renderQueue,
            
            ILogger log)
        {
            var meetupPageModel = new MeetupPage();
            meetupPageModel.Registration = new MeetupRegistration
            {
                IsOpened = true,
                Url = null
            };
            meetupPageModel.Meetup = 
                (await meetupTable.GetQueryResultsAsync<Meetup>(
                    rowKey: command.MeetupId.ToString("N")))
                .Single();
            meetupPageModel.Venue = (await venueTable.GetQueryResultsAsync<Venue>(
                    string.Empty, 
                    meetupPageModel.Meetup.VenueId.ToString("N")))
                .Single();
            
            
            var meetupSessions = (await meetupSessionTable.GetQueryResultsAsync<MeetupSession>(
                    command.MeetupId.ToString("N")))
                .OrderBy(x => x.OrderN)
                .ToList();

            var agenda = new List<MeetupAgenda>();
            foreach (var meetupSession in meetupSessions)
            {
                var sessionInfo = (await sessionTable.GetQueryResultsAsync<Session>(
                        rowKey: meetupSession.SessionId.ToString("N")))
                    .Single();

                var speakerInfo = (await speakerTable.GetQueryResultsAsync<Speaker>(
                        rowKey: sessionInfo.SpeakerId.ToString("N")))
                    .Single();
                
                agenda.Add(new MeetupAgenda
                {
                    MeetupSession = meetupSession,
                    Session = sessionInfo,
                    Speaker = speakerInfo
                });
            }

            meetupPageModel.Sessions = agenda.ToArray();
            
            meetupPageModel.Partners = new IPartner[0];

            await meetupPageDataBlob.UploadTextAsync(JsonConvert.SerializeObject(meetupPageModel));
            
            var renderCommand = new RenderPage
            {
                DataInstanceId = $"meetup-{command.MeetupId}.json",
                PublicUrl = meetupPageModel.PublicUrl,
                TemplateId = MeetupPage.TemplateId
            };
            await renderQueue.AddMessageAsync(renderCommand.ToQueueMessage());
        }
    }
}