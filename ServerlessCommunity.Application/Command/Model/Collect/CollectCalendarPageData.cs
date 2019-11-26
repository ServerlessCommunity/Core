using ServerlessCommunity.Application.Command.Collect.Base;

namespace ServerlessCommunity.Application.Command.Collect
{
    public class CollectCalendarPageData : CollectCommandBase
    {
        public override string DataInstanceId => "calendar.json";
    }
}