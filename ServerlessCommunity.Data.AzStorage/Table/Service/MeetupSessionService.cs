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
    public sealed class MeetupSessionService : TableStorageServiceBase<MeetupSession>, IMeetupSessionService
    {
        public MeetupSessionService(CloudTable meetupSessionTable)
            : base(meetupSessionTable)
        {
        }

        public async Task<IList<IMeetupSession>> GetMeetupSessionsAsync()
        {
            return (await GetQueryResultsAsync())
                .OrderBy(x => x.OrderN)
                .Cast<IMeetupSession>()
                .ToList();
        }

        public async Task<IList<IMeetupSession>> GetMeetupSessionsByMeetupIdsAsync(IEnumerable<string> ids)
        {
            var filters = SplitFilterForSegments(ids, nameof(ITableEntity.PartitionKey));

            return (await GetQueryResultsAsync(filters))
                .Cast<IMeetupSession>()
                .ToList();
        }

        public async Task<IList<IMeetupSession>> GetMeetupSessionsByMeetupIdsAsync(IEnumerable<Guid> ids)
        {
            return await GetMeetupSessionsByMeetupIdsAsync(ids.Select(x => x.ToString("N")));
        }

        public async Task<IList<IMeetupSession>> GetMeetupSessionsByMeetupIdAsync(string id)
        {
            var filter = GenerateCondition(nameof(ITableEntity.PartitionKey), id);

            return (await GetQueryResultsAsync(filter))
                .OrderBy(x => x.OrderN)
                .Cast<IMeetupSession>()
                .ToList();
        }

        public async Task<IList<IMeetupSession>> GetMeetupSessionsByMeetupIdAsync(Guid id)
        {
            return await GetMeetupSessionsByMeetupIdAsync(id.ToString("N"));
        }
    }
}