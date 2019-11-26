using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Application.Data.Service
{
    public interface IMeetupSessionService
    {
        Task<IList<IMeetupSession>> GetMeetupSessionsAsync();
        
        Task<IList<IMeetupSession>> GetMeetupSessionsByMeetupIdsAsync(IEnumerable<string> ids);
        Task<IList<IMeetupSession>> GetMeetupSessionsByMeetupIdsAsync(IEnumerable<Guid> ids);
        
        Task<IList<IMeetupSession>> GetMeetupSessionsByMeetupIdAsync(string id);
        Task<IList<IMeetupSession>> GetMeetupSessionsByMeetupIdAsync(Guid id);
    }
}