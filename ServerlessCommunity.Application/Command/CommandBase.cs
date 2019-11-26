using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace ServerlessCommunity.Application.Command
{
    public abstract class CommandBase
    {
        public CloudQueueMessage ToQueueMessage()
        {
            return new CloudQueueMessage(JsonConvert.SerializeObject(this));
        }
    }
}