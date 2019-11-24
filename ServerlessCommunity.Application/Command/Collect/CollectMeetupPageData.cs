using System;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace ServerlessCommunity.Application.Command.Collect
{
    public class CollectMeetupPageData
    {
        public Guid MeetupId { get; set; }
        
        public CloudQueueMessage ToQueueMessage()
        {
            return new CloudQueueMessage(JsonConvert.SerializeObject(this));
        }
    }
}