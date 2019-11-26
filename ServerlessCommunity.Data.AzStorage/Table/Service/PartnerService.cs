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
    public class PartnerService : TableStorageServiceBase<Partner>, IPartnerService
    {
        public PartnerService(CloudTable table)
            : base(table)
        {
        }

        public async Task<IList<IPartner>> GetPartnersHighlightedAsync(bool isHighlighted = true)
        {
            var filter = GenerateConditionBoolean(nameof(IHighlighted.IsHighlighted), isHighlighted);
            
            return (await GetQueryResultsAsync(filter))
                .OrderBy(x => x.Title)
                .Cast<IPartner>()
                .ToList();
        }
    }
}