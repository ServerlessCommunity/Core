using System;

namespace ServerlessCommunity.Application.Data
{
    public interface IMeetupSession
    {
        Guid MeetupId { get; set; }

        Guid SessionId { get; set; }
        
        int OrderN { get; set; }
    }
}