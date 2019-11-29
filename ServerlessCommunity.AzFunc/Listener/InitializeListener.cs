using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Command.Collect;
using ServerlessCommunity.Config;
using ServerlessCommunity.Data.AzStorage.Queue.Service;
using ServerlessCommunity.Data.AzStorage.Table.Service;

namespace ServerlessCommunity.AzFunc.Listener
{
    public static class InitializeListener
    {
        [FunctionName(nameof(InitializeListener))]
        public static async Task Run(
            [QueueTrigger(QueueName.ListenInitialize)]string command,
            
            [Table(TableName.Meetup)] CloudTable meetupTable,
            
            [Queue(QueueName.CollectMeetupPage)]CloudQueue meetupQueue,
            [Queue(QueueName.CollectCalendarPage)]CloudQueue calendarQueue,
            [Queue(QueueName.CollectHomePage)]CloudQueue homepageQueue)
        {
            var meetupService = new MeetupService(meetupTable);

            var collectMeetupService = new CommandQueueService(meetupQueue);
            var collectCalendarService = new CommandQueueService(calendarQueue);
            var collectHomepageService = new CommandQueueService(homepageQueue);
            
            var meetups = await meetupService.GetMeetupsAsync();

            foreach (var meetup in meetups)
            {
                await collectMeetupService.SubmitCommandAsync(new CollectMeetupPageData
                {
                    MeetupId = meetup.Id
                });
            }

            await collectCalendarService.SubmitCommandAsync(new CollectCalendarPageData());
            await collectHomepageService.SubmitCommandAsync(new CollectHomePageData());
        }
    }
}