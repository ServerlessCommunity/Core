using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Command.Collect;
using ServerlessCommunity.Config;
using ServerlessCommunity.AzFunc._Extensions;
using ServerlessCommunity.Data.AzStorage.Table;
using ServerlessCommunity.Data.AzStorage.Table.Model;

namespace ServerlessCommunity.AzFunc.Listener
{
    public static class InitializeListener
    {
        [FunctionName(nameof(ListenInitializeFunction))]
        public static async Task ListenInitializeFunction(
            [QueueTrigger(QueueName.ListenInitialize)]string command,
            
            [Table(TableName.Meetup)] CloudTable meetupTable,
            
            [Queue(QueueName.CollectMeetupPage)]CloudQueue meetupQueue,
            [Queue(QueueName.CollectCalendarPage)]CloudQueue calendarQueue,
            [Queue(QueueName.CollectHomePage)]CloudQueue homepageQueue)
        {
            var meetups = await meetupTable.GetQueryResultsAsync<Meetup>();

            foreach (var meetup in meetups)
            {
                await meetupQueue.AddMessageAsync(new CollectMeetupPageData
                {
                    MeetupId = meetup.Id
                }.ToQueueMessage());
            }

            await calendarQueue.AddMessageAsync(new CollectCalendarPageData().ToQueueMessage());
            await homepageQueue.AddMessageAsync(new CollectHomePageData().ToQueueMessage());
        }
    }
}