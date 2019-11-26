using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessCommunity.Application.Data.Model;

namespace ServerlessCommunity.Application.Data.Service
{
    public interface ISpeakerService
    {
        Task<IList<ISpeaker>> GetSpeakersAsync();
        
        Task<IList<ISpeaker>> GetSpeakersHighlightedAsync(bool isHighlighted = true);
        
        Task<IList<ISpeaker>> GetSpeakersByIdsAsync(IEnumerable<string> ids);
        Task<IList<ISpeaker>> GetSpeakersByIdsAsync(IEnumerable<Guid> ids);
    }
}