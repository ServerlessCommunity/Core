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
using ServerlessCommunity.Application.Data.Model;
using ServerlessCommunity.Application.ViewModel.Meetup;
using ServerlessCommunity.Config;
using ServerlessCommunity.Data.AzStorage.Queue.Service;
using ServerlessCommunity.Data.AzStorage.Table.Service;

namespace ServerlessCommunity.AzFunc.Collector
{
    public static class MeetupPageDataCollector
    {
        private const string PageDataBlobUrl = "/meetup-{" + nameof(CollectMeetupPageData.MeetupId) + "}.json";
        
        [FunctionName(nameof(MeetupPageDataCollector))]
        public static async Task CollectMeetupPageDataFunction(
            [QueueTrigger(QueueName.CollectMeetupPage)]CollectMeetupPageData command,
            
            [Table(TableName.Meetup)] CloudTable meetupTable,
            [Table(TableName.MeetupSession)] CloudTable meetupSessionTable,
            [Table(TableName.Session)] CloudTable sessionTable,
            [Table(TableName.Speaker)] CloudTable speakerTable,
            [Table(TableName.Venue)] CloudTable venueTable,
            
            [Blob(ContainerName.PageData + PageDataBlobUrl, FileAccess.Write)]CloudBlockBlob meetupPageDataBlob,
            [Queue(QueueName.RenderPage)]CloudQueue renderQueue,
            
            ILogger log)
        {
            var meetupService = new MeetupService(meetupTable);
            var venueService = new VenueService(venueTable);
            var meetupSessionService = new MeetupSessionService(meetupSessionTable);
            var sessionService = new SessionService(sessionTable);
            var speakerService = new SpeakerService(speakerTable);

            var renderService = new CommandQueueService(renderQueue);
            
            var meetupPageViewModel = new MeetupPage();
            meetupPageViewModel.Registration = new MeetupRegistration
            {
                IsOpened = true,
                Url = null
            };
            
            meetupPageViewModel.Meetup = await meetupService.GetMeetupByIdAsync(command.MeetupId);
            meetupPageViewModel.Venue = await venueService.GetVenueByIdAsync(meetupPageViewModel.Meetup.VenueId);


            var meetupAgendaItems = await meetupSessionService.GetMeetupSessionsByMeetupIdAsync(meetupPageViewModel.Meetup.Id);
            var sessions = await sessionService.GetSessionsByIdsAsync(meetupAgendaItems.Select(x => x.SessionId));
            var speakers = await speakerService.GetSpeakersByIdsAsync(sessions.Select(x => x.SpeakerId));

            meetupPageViewModel.Sessions = meetupAgendaItems.Select(x =>
            {
                var agenda = new MeetupAgenda
                {
                    MeetupSession = x,
                    Session = sessions.Single(s => s.Id == x.SessionId)
                };

                agenda.Speaker = speakers.Single(s => s.Id == agenda.Session.SpeakerId);

                return agenda;
            }).ToArray();
            
            meetupPageViewModel.Partners = new IPartner[0];

            await meetupPageDataBlob.UploadTextAsync(JsonConvert.SerializeObject(meetupPageViewModel));
            
            var renderCommand = new RenderPage
            {
                DataInstanceId = command.DataInstanceId,
                PublicUrl = meetupPageViewModel.PublicUrl,
                TemplateId = MeetupPage.TemplateId
            };
            await renderService.SubmitCommandAsync(renderCommand);
        }
    }
}