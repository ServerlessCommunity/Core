using System;
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
using ServerlessCommunity.Application.ViewModel.Home;
using ServerlessCommunity.Application.ViewModel.Meetup;
using ServerlessCommunity.Config;
using ServerlessCommunity.AzFunc._Extensions;

namespace ServerlessCommunity.AzFunc.Collector
{
    public static class HomePageDataCollector
    {
        public const int TimeZoneOffset = +2;
        public const int TopMeetups = 3;
        
        [FunctionName(nameof(HomePageDataCollector))]
        public static async Task CollectHomePageDataFunction(
            [QueueTrigger(QueueName.CollectHomePage)]CollectHomePageData command,
            
            [Table(TableName.Meetup)] CloudTable meetupTable,
            [Table(TableName.MeetupSession)] CloudTable meetupSessionTable,
            [Table(TableName.Session)] CloudTable sessionTable,
            [Table(TableName.Speaker)] CloudTable speakerTable,
            [Table(TableName.Venue)] CloudTable venueTable,
            
            [Blob(ContainerName.PageData + "/index.json", FileAccess.Write)]CloudBlockBlob homePageDataBlob,
            [Queue(QueueName.RenderPage)]CloudQueue renderQueue,
            
            ILogger log)
        {
            var homePageModel = new HomePage();

            var date = DateTime.UtcNow.AddHours(TimeZoneOffset);            
            var meetups = (await meetupTable.GetQueryResultsAsync<Meetup>())
                .Where(x => x.Year >= date.Year 
                            && x.Month >= date.Month
                            && x.Day >= date.Day)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenByDescending(x => x.Day)
                .Take(TopMeetups)
                .ToArray();

            var meetupPages = new List<MeetupPage>();
            foreach (var meetup in meetups)
            {
                var meetupPage = new MeetupPage();
                meetupPages.Add(meetupPage);
                
                meetupPage.Meetup = meetup;
                
                meetupPage.Registration = new MeetupRegistration
                {
                    IsOpened = true,
                    Url = null
                };
                
                meetupPage.Venue = (await venueTable.GetQueryResultsAsync<Venue>(
                        string.Empty, 
                        meetupPage.Meetup.VenueId.ToString("N")))
                    .Single();
                
                var meetupSessions = (await meetupSessionTable.GetQueryResultsAsync<MeetupSession>(
                        meetupPage.Meetup.Id.ToString("N")))
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
                
                meetupPage.Sessions = agenda.ToArray();
                
                meetupPage.Partners = new Partner[0];
            }

            homePageModel.Meetups = meetupPages.ToArray();

            homePageModel.Speakers = (await speakerTable.GetHighlightedAsync<Speaker>())
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToArray();

            homePageModel.Partners = (await speakerTable.GetHighlightedAsync<Partner>())
                .OrderBy(x => x.Title)
                .ToArray();
            
            await homePageDataBlob.UploadTextAsync(JsonConvert.SerializeObject(homePageModel));
            var renderCommand = new RenderPage
            {
                DataInstanceId = "index.json",
                PublicUrl = homePageModel.PublicUrl,
                TemplateId = HomePage.TemplateId
            };
            await renderQueue.AddMessageAsync(renderCommand.ToQueueMessage());
        }
    }
}