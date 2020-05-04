using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using ServerlessCommunity.Application.Command;
using ServerlessCommunity.Application.Command.Service;

namespace ServerlessCommunity.Data.AzStorage.Queue.Service
{
    public sealed class CommandQueueService : ICommandService
    {
        private readonly CloudQueue _queue;
        
        public CommandQueueService(CloudQueue queue)
        {
            _queue = queue;
        }

        public async Task SubmitCommandAsync(CommandBase command, TimeSpan? delay = null)
        {
            var payload = JsonConvert.SerializeObject(command);
            var message = new CloudQueueMessage(payload);

            await _queue.AddMessageAsync(message, null, delay, null, null);
        }
    }
}