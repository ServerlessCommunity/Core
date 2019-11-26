using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Application.Data.Service
{
    public interface ISessionService
    {
        Task<IList<ISession>> GetSessionsAsync();
        
        Task<IList<ISession>> GetSessionsByIdsAsync(IEnumerable<string> ids);
        Task<IList<ISession>> GetSessionsByIdsAsync(IEnumerable<Guid> ids);
    }
}