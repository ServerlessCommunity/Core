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
using ServerlessCommunity.Application.ViewModel.Calendar;
using ServerlessCommunity.Application.ViewModel.Meetup;
using ServerlessCommunity.Config;
using ServerlessCommunity.Data.AzStorage.Queue.Service;
using ServerlessCommunity.Data.AzStorage.Table.Service;

namespace ServerlessCommunity.AzFunc.Collector
{
    public static class CalendarPageDataCollector
    {
        private const string PageDataBlobUrl = "/calendar.json";
        
        [FunctionName(nameof(CalendarPageDataCollector))]
        public static async Task Run(
            [QueueTrigger(QueueName.CollectCalendarPage)]CollectCalendarPageData command,
            
            [Table(TableName.Meetup)] CloudTable meetupTable,
            [Table(TableName.MeetupSession)] CloudTable meetupSessionTable,
            [Table(TableName.Session)] CloudTable sessionTable,
            [Table(TableName.Speaker)] CloudTable speakerTable,
            [Table(TableName.Venue)] CloudTable venueTable,
            
            [Blob(ContainerName.PageData + PageDataBlobUrl, FileAccess.Write)]CloudBlockBlob calendarPageDataBlob,
            [Queue(QueueName.RenderPage)]CloudQueue renderQueue,
            
            ILogger log)
        {
            var meetupService = new MeetupService(meetupTable);
            var venueService = new VenueService(venueTable);
            var meetupSessionService = new MeetupSessionService(meetupSessionTable);
            var sessionService = new SessionService(sessionTable);
            var speakerService = new SpeakerService(speakerTable);

            var renderService = new CommandQueueService(renderQueue);
            
            
            var calendarPageModel = new CalendarPage();

            var meetups = (await meetupService.GetMeetupsAsync())
                .GroupBy(x => x.Year, (key, elements) => new
                {
                    year = key,
                    meetups = elements
                })
                .ToDictionary(x => x.year, element => element.meetups);

            var meetupSessions = await meetupSessionService.GetMeetupSessionsAsync();
            var sessions = await sessionService.GetSessionsAsync();
            var speakers = await speakerService.GetSpeakersAsync();
            var venues = await venueService.GetVenuesAsync();

            calendarPageModel.Years = meetups.Keys.ToArray();
            calendarPageModel.CalendarYears = meetups.Values.Select(x =>
                {
                    var calendarYear = new CalendarYear();

                    calendarYear.Year = x.First().Year;
                    calendarYear.Meetups = x.Select(y =>
                        {
                            var meetupPage = new MeetupPage();

                            meetupPage.Meetup = y;

                            meetupPage.Venue = venues
                                .Single(z => z.Id == meetupPage.Meetup.VenueId);

                            meetupPage.Sessions = meetupSessions
                                .Where(z => z.MeetupId == meetupPage.Meetup.Id)
                                .OrderBy(z => z.OrderN)
                                .Select(z =>
                                {
                                    var agenda = new MeetupAgenda();

                                    agenda.MeetupSession = z;

                                    agenda.Session = sessions
                                        .Single(q => q.Id == agenda.MeetupSession.SessionId);

                                    agenda.Speaker = speakers
                                        .Single(q => q.Id == agenda.Session.SpeakerId);
                                    
                                    return agenda;
                                })
                                .ToArray();
                            
                            return meetupPage;
                        })
                        .ToArray();
                    
                    return calendarYear;
                })
                .ToArray();


            await calendarPageDataBlob.UploadTextAsync(JsonConvert.SerializeObject(calendarPageModel));
            
            var renderCommand = new RenderPage
            {
                DataInstanceId = command.DataInstanceId,
                PublicUrl = calendarPageModel.PublicUrl,
                TemplateId = CalendarPage.TemplateId
            };
            await renderService.SubmitCommandAsync(renderCommand);
        }
    }
}