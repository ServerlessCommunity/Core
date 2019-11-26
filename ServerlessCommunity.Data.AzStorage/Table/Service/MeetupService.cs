using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Data.Model;
using ServerlessCommunity.Application.Data.Service;
using ServerlessCommunity.Data.AzStorage.Table.Model;
using ServerlessCommunity.Data.AzStorage.Table.Service.Base;

namespace ServerlessCommunity.Data.AzStorage.Table.Service
{
    public sealed class MeetupService : TableStorageServiceBase<Meetup>, IMeetupService
    {
        public MeetupService(CloudTable meetupTable)
            : base(meetupTable)
        {
        }
        
        public async Task<IList<IMeetup>> GetMeetupsAsync()
        {
            return (await GetQueryResultsAsync())
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenByDescending(x => x.Day)
                .Cast<IMeetup>()
                .ToList();
        }

        public async Task<IList<IMeetup>> GetMeetupsUpcomingAsync(int top, DateTime? currentDate = null)
        {
            var filter = GenerateCondition(
                nameof(ITableEntity.PartitionKey),
                (currentDate ?? DateTime.UtcNow).Year.ToString(),
                QueryComparisons.GreaterThanOrEqual);
            
            return (await GetQueryResultsAsync(filter))
                .Take(top)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenByDescending(x => x.Day)
                .Cast<IMeetup>()
                .ToList();
        }

        public async Task<IMeetup> GetMeetupByIdAsync(string id)
        {
            return await GetEntityById(rowKey: id);
        }

        public async Task<IMeetup> GetMeetupByIdAsync(Guid id)
        {
            return await GetMeetupByIdAsync(id.ToString("N"));
        }
    }
}