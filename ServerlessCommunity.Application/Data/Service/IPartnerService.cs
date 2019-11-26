using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Application.Data.Service
{
    public interface IPartnerService
    {
        Task<IList<IPartner>> GetPartnersHighlightedAsync(bool isHighlighted = true);
    }
}