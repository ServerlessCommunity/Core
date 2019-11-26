using System;
using ServerlessCommunity.Application.Command.Collect.Base;

namespace ServerlessCommunity.Application.Command.Collect
{
    public class CollectMeetupPageData : CollectCommandBase
    {
        public Guid MeetupId { get; set; }
        
        public override string DataInstanceId => $"meetup-{MeetupId:N}.json";
    }
}