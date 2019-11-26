namespace ServerlessCommunity.Application.Command.Collect.Base
{
    public abstract class CollectCommandBase : CommandBase
    {
        public abstract string DataInstanceId { get; }
    }
}