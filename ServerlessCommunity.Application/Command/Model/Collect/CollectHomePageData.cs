using ServerlessCommunity.Application.Command.Collect.Base;

namespace ServerlessCommunity.Application.Command.Collect
{
    public class CollectHomePageData : CollectCommandBase
    {
        public override string DataInstanceId => "index.json";
    }
}