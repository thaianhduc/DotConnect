namespace DotConnect.Core.Interfaces
{
    public interface ICommandHandler<in TCommand, out TCommandResult>
        where TCommand : class 
        where TCommandResult : class 
    {
        TCommandResult Handle(TCommand command);
    }
}
