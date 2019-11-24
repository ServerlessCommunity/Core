using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace ServerlessCommunity.Application.Command.Collect
{
    public class CollectHomePageData
    {
        public CloudQueueMessage ToQueueMessage()
        {
            return new CloudQueueMessage(JsonConvert.SerializeObject(this));
        }
    }
}