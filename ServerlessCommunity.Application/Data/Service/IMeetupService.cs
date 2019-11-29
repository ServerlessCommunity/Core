using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Application.Data.Service
{
    public interface IMeetupService
    {
        Task<IList<IMeetup>> GetMeetupsAsync();

        Task<IList<IMeetup>> GetMeetupsUpcomingAsync(int top, DateTime? currentDate = null);
        
        Task<IList<IMeetup>> GetMeetupsForDateAsync(DateTime? date = null);
        
        Task<IMeetup> GetMeetupByIdAsync(string id);
        Task<IMeetup> GetMeetupByIdAsync(Guid id);
    }
}