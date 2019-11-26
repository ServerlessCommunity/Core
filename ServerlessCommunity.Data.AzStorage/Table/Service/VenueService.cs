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
    public sealed class VenueService : TableStorageServiceBase<Venue>, IVenueService
    {
        public VenueService(CloudTable venueTable)
            : base(venueTable)
        {
        }

        public async Task<IList<IVenue>> GetVenuesAsync()
        {
            return (await GetQueryResultsAsync())
                .Cast<IVenue>()
                .ToList();
        }

        public async Task<IList<IVenue>> GetVenuesByIdsAsync(IEnumerable<string> ids)
        {
            var filters = SplitFilterForSegments(ids, nameof(ITableEntity.RowKey));

            return (await GetQueryResultsAsync(filters))
                .Cast<IVenue>()
                .ToList();
        }

        public async Task<IList<IVenue>> GetVenuesByIdsAsync(IEnumerable<Guid> ids)
        {
            return await GetVenuesByIdsAsync(ids.Select(x => x.ToString("N")));
        }

        public async Task<IVenue> GetVenueByIdAsync(string id)
        {
            return await GetEntityById(rowKey: id);
        }

        public async Task<IVenue> GetVenueByIdAsync(Guid id)
        {
            return await GetVenueByIdAsync(id.ToString("N"));
        }
    }
}