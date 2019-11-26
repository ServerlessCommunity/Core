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
    public sealed class SessionService : TableStorageServiceBase<Session>, ISessionService
    {
        public SessionService(CloudTable sessionTable)
            : base(sessionTable)
        {
        }

        public async Task<IList<ISession>> GetSessionsAsync()
        {
            return (await GetQueryResultsAsync())
                .Cast<ISession>()
                .ToList();
        }

        public async Task<IList<ISession>> GetSessionsByIdsAsync(IEnumerable<string> ids)
        {
            var filters = SplitFilterForSegments(ids, nameof(ITableEntity.RowKey));

            return (await GetQueryResultsAsync(filters))
                .Cast<ISession>()
                .ToList();
        }

        public async Task<IList<ISession>> GetSessionsByIdsAsync(IEnumerable<Guid> ids)
        {
            return await GetSessionsByIdsAsync(ids.Select(x => x.ToString("N")));
        }
    }
}