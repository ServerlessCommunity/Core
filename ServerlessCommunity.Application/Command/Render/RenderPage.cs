using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace ServerlessCommunity.Application.Command.Render
{
    public class RenderPage
    {
        public string DataInstanceId { get; set; }
        public string PublicUrl { get; set; }
        public string TemplateId { get; set; }
        
        public CloudQueueMessage ToQueueMessage()
        {
            return new CloudQueueMessage(JsonConvert.SerializeObject(this));
        }
    }
}
