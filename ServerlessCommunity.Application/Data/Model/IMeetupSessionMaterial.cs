using System;

namespace ServerlessCommunity.Application.Data.Model
{
    public interface IMeetupSessionMaterial
    {
        Guid MeetupId { get; set; }
        Guid SessionId { get; set; }
        int OrderN { get; set; }
        string Title { get; set; }
        string Url { get; set; }
    }
}