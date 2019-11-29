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
                .OrderByDescending(x => x.DateFormatted)
                .ThenByDescending(x => x.TimeStartFormatted)
                .Cast<IMeetup>()
                .ToList();
        }

        public async Task<IList<IMeetup>> GetMeetupsUpcomingAsync(int top, DateTime? currentDate = null)
        {
            currentDate = currentDate ?? DateTime.UtcNow;
            
            var filter = GenerateCondition(
                nameof(ITableEntity.PartitionKey),
                currentDate.Value.Year.ToString(),
                QueryComparisons.GreaterThanOrEqual);
            
            return (await GetQueryResultsAsync(filter))
                .Where(x => 
                    x.Month >= currentDate.Value.Month 
                    && x.Day >= currentDate.Value.Day)
                .OrderByDescending(x => x.DateFormatted)
                .ThenByDescending(x => x.TimeStartFormatted)
                .Take(top)
                .Cast<IMeetup>()
                .ToList();
        }

        public async Task<IList<IMeetup>> GetMeetupsForDateAsync(DateTime? date = null)
        {
            date = date ?? DateTime.UtcNow;
            
            var filter = GenerateCondition(
                nameof(ITableEntity.PartitionKey),
                date.Value.Year.ToString());

            filter = AppendFilter(filter, TableOperators.And, GenerateConditionInt(
                nameof(IMeetup.Month),
                date.Value.Month));
            
            filter = AppendFilter(filter, TableOperators.And, GenerateConditionInt(
                nameof(IMeetup.Day),
                date.Value.Day));
            
            return (await GetQueryResultsAsync(filter))
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