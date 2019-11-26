using System;

namespace ServerlessCommunity.Application.Data.Model
{
    public interface IMeetupSession
    {
        Guid MeetupId { get; set; }

        Guid SessionId { get; set; }
        
        int OrderN { get; set; }
    }
}