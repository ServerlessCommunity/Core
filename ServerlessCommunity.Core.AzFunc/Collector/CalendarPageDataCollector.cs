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
using ServerlessCommunity.Application.Model.Calendar;
using ServerlessCommunity.Application.Model.Meetup;
using ServerlessCommunity.Config;
using ServerlessCommunity.Core.AzFunc._Extensions;

namespace ServerlessCommunity.Core.AzFunc.Collector
{
    public static class CalendarPageDataCollector
    {
        [FunctionName(nameof(CalendarPageDataCollector))]
        public static async Task CollectCalendarPageDataFunction(
            [QueueTrigger(QueueName.CollectCalendarPage)]CollectCalendarPageData command,
            
            [Table(TableName.Meetup)] CloudTable meetupTable,
            [Table(TableName.MeetupSession)] CloudTable meetupSessionTable,
            [Table(TableName.Session)] CloudTable sessionTable,
            [Table(TableName.Speaker)] CloudTable speakerTable,
            [Table(TableName.Venue)] CloudTable venueTable,
            
            [Blob(ContainerName.PageData + "/calendar.json", FileAccess.Write)]CloudBlockBlob calendarPageDataBlob,
            [Queue(QueueName.RenderPage)]CloudQueue renderQueue,
            
            ILogger log)
        {
            var calendarPageModel = new CalendarPage();

            var meetups = (await meetupTable.GetQueryResultsAsync<Meetup>())
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenByDescending(x => x.Day)
                .GroupBy(x => x.Year, (key, elements) => new
                {
                    year = key,
                    meetups = elements
                })
                .ToDictionary(x => x.year, element => element.meetups);
            var meetupSessions = await meetupSessionTable.GetQueryResultsAsync<MeetupSession>();
            var sessions = await sessionTable.GetQueryResultsAsync<Session>();
            var speakers = await speakerTable.GetQueryResultsAsync<Speaker>();
            var venues = await venueTable.GetQueryResultsAsync<Venue>();

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
                DataInstanceId = "calendar.json",
                PublicUrl = calendarPageModel.PublicUrl,
                TemplateId = CalendarPage.TemplateId
            };
            await renderQueue.AddMessageAsync(renderCommand.ToQueueMessage());
        }
    }
}