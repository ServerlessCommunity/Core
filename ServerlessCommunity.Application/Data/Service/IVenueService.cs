using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Application.Data.Service
{
    public interface IVenueService
    {
        Task<IList<IVenue>> GetVenuesAsync();
        
        Task<IList<IVenue>> GetVenuesByIdsAsync(IEnumerable<string> ids);
        Task<IList<IVenue>> GetVenuesByIdsAsync(IEnumerable<Guid> ids);

        Task<IVenue> GetVenueByIdAsync(string id);
        Task<IVenue> GetVenueByIdAsync(Guid id);
    }
}