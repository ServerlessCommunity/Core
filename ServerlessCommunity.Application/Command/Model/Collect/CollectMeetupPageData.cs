using System;
using ServerlessCommunity.Application.Command.Collect.Base;

namespace ServerlessCommunity.Application.Command.Collect
{
    public class CollectMeetupPageData : CollectCommandBase
    {
        private Guid _meetupId;

        public string MeetupId
        {
            get => _meetupId.ToString("N");
            set => _meetupId = Guid.Parse(value);
        }

        public Guid Id
        {
            get => _meetupId;
            set => _meetupId = value;
        }

        public override string DataInstanceId => $"meetup-{MeetupId}.json";
    }
}