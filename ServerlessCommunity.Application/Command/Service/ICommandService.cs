using System.Threading.Tasks;

namespace ServerlessCommunity.Application.Command.Service
{
    public interface ICommandService
    {
        Task SubmitCommandAsync(CommandBase command);
    }
}