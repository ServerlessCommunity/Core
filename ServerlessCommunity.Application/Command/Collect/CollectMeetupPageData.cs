using System;

namespace ServerlessCommunity.Application.Command.Collect
{
    public class CollectMeetupPageData : CommandBase
    {
        public Guid MeetupId { get; set; }
    }
}