using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Command.Collect;
using ServerlessCommunity.Config;
using ServerlessCommunity.Data.AzStorage.Queue.Service;
using ServerlessCommunity.Data.AzStorage.Table.Service;

namespace ServerlessCommunity.AzFunc.Timer
{
    public static class TimerListener
    {
        [FunctionName(nameof(TimerListener))]
        public static async Task Run(
            [TimerTrigger("0 1 0 * * *")]TimerInfo myTimer,
            
            [Table(TableName.Meetup)] CloudTable meetupTable,
            
            [Queue(QueueName.CollectMeetupPage)]CloudQueue meetupQueue,
            
            ILogger log)
        {
            var meetupService = new MeetupService(meetupTable);
            var meetupCommands = new CommandQueueService(meetupQueue);

            var meetups = await meetupService.GetMeetupsForDateAsync(myTimer.ScheduleStatus.Last);

            foreach (var meetup in meetups)
            {
                var delay = TimeSpan
                    .FromHours(meetup.TimeStartHours)
                    .Add(TimeSpan.FromMinutes(meetup.TimeStartMinutes))
                    .Subtract(TimeSpan.FromMinutes(10));

                var payload = new CollectMeetupPageData
                {
                    Id = meetup.Id
                };

                await meetupCommands.SubmitCommandAsync(payload, delay);
            }
        }
    }
}