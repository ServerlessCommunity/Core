using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Application.Data.Service
{
    public interface IMeetupSessionMaterialService
    {
        Task<IList<IMeetupSessionMaterial>> GetMeetupSessionsMaterialsByMeetupIdAsync(string id);
        Task<IList<IMeetupSessionMaterial>> GetMeetupSessionsMaterialsByMeetupIdAsync(Guid id);
    }
}