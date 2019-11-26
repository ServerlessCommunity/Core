using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessCommunity.Application.Data.Model;
using ServerlessCommunity.Application.Data.Model.Base;
using ServerlessCommunity.Application.Data.Service;
using ServerlessCommunity.Data.AzStorage.Table.Model;
using ServerlessCommunity.Data.AzStorage.Table.Service.Base;

namespace ServerlessCommunity.Data.AzStorage.Table.Service
{
    public sealed class SpeakerService : TableStorageServiceBase<Speaker>, ISpeakerService
    {
        public SpeakerService(CloudTable table)
            : base(table)
        {
        }

        public async Task<IList<ISpeaker>> GetSpeakersAsync()
        {
            return (await GetQueryResultsAsync())
                .Cast<ISpeaker>()
                .ToList();
        }

        public async Task<IList<ISpeaker>> GetSpeakersHighlightedAsync(bool isHighlighted = true)
        {
            var filter = GenerateConditionBoolean(nameof(IHighlighted.IsHighlighted), isHighlighted);
            
            return (await GetQueryResultsAsync(filter))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Cast<ISpeaker>()
                .ToList();
        }

        public async Task<IList<ISpeaker>> GetSpeakersByIdsAsync(IEnumerable<string> ids)
        {
            var filters = SplitFilterForSegments(ids, nameof(ITableEntity.RowKey));

            return (await GetQueryResultsAsync(filters))
                .Cast<ISpeaker>()
                .ToList();
        }

        public async Task<IList<ISpeaker>> GetSpeakersByIdsAsync(IEnumerable<Guid> ids)
        {
            return await GetSpeakersByIdsAsync(ids.Select(x => x.ToString("N")));
        }
    }
}