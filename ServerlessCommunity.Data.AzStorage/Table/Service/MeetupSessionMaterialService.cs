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
    public class MeetupSessionMaterialService : TableStorageServiceBase<MeetupSessionMaterial>, IMeetupSessionMaterialService
    {
        public MeetupSessionMaterialService(CloudTable table)
            : base(table)
        {
        }

        public async Task<IList<IMeetupSessionMaterial>> GetMeetupSessionsMaterialsByMeetupIdAsync(string id)
        {
            var filter = GenerateCondition(nameof(ITableEntity.PartitionKey), id);
            
            var results = (await GetQueryResultsAsync(filter))
                .OrderBy(x => x.OrderN)
                .Cast<IMeetupSessionMaterial>()
                .ToList();

            return results;
        }

        public async Task<IList<IMeetupSessionMaterial>> GetMeetupSessionsMaterialsByMeetupIdAsync(Guid id)
        {
            return await GetMeetupSessionsMaterialsByMeetupIdAsync(id.ToString("N"));
        }
    }
}