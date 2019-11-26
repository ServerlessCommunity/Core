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
using ServerlessCommunity.Application.Data.Model;
using ServerlessCommunity.Application.ViewModel.Home;
using ServerlessCommunity.Application.ViewModel.Meetup;
using ServerlessCommunity.Config;
using ServerlessCommunity.Data.AzStorage.Queue.Service;
using ServerlessCommunity.Data.AzStorage.Table.Service;

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
            [Table(TableName.Partner)] CloudTable partnerTable,
            
            [Blob(ContainerName.PageData + "/index.json", FileAccess.Write)]CloudBlockBlob homePageDataBlob,
            [Queue(QueueName.RenderPage)]CloudQueue renderQueue,
            
            ILogger log)
        {
            var meetupService = new MeetupService(meetupTable);
            var venueService = new VenueService(venueTable);
            var meetupSessionService = new MeetupSessionService(meetupSessionTable);
            var sessionService = new SessionService(sessionTable);
            var speakerService = new SpeakerService(speakerTable);
            var partnerService = new PartnerService(partnerTable);

            var renderService = new CommandQueueService(renderQueue);
            
            
            var homePageModel = new HomePage();

            var date = DateTime.UtcNow.AddHours(TimeZoneOffset);
            var meetups = await meetupService.GetMeetupsUpcomingAsync(TopMeetups, date);

            var venues = await venueService.GetVenuesByIdsAsync(meetups.Select(x => x.VenueId));
            var agendaItems = await meetupSessionService.GetMeetupSessionsByMeetupIdsAsync(meetups.Select(x => x.Id));
            var sessions = await sessionService.GetSessionsByIdsAsync(agendaItems.Select(x => x.SessionId));
            var speakers = await speakerService.GetSpeakersByIdsAsync(sessions.Select(x => x.SpeakerId));

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

                meetupPage.Venue = venues.Single(x => x.Id == meetup.VenueId);
                
                var agenda = agendaItems
                    .Where(x => x.MeetupId == meetup.Id)
                    .Select(a =>
                    {
                        var y = new MeetupAgenda
                        {
                            MeetupSession = a,
                            Session = sessions.Single(x => x.Id == a.SessionId)
                        };

                        y.Speaker = speakers.Single(x => x.Id == y.Session.SpeakerId);

                        return y;
                    });
                
                meetupPage.Sessions = agenda.ToArray();
                
                meetupPage.Partners = new IPartner[0];
            }

            homePageModel.Meetups = meetupPages.ToArray();

            homePageModel.Speakers = (await speakerService.GetSpeakersHighlightedAsync())
                .ToArray();

            homePageModel.Partners = (await partnerService.GetPartnersHighlightedAsync())
                .ToArray();
            
            await homePageDataBlob.UploadTextAsync(JsonConvert.SerializeObject(homePageModel));
            
            var renderCommand = new RenderPage
            {
                DataInstanceId = command.DataInstanceId,
                PublicUrl = homePageModel.PublicUrl,
                TemplateId = HomePage.TemplateId
            };
            await renderService.SubmitCommandAsync(renderCommand);
        }
    }
}