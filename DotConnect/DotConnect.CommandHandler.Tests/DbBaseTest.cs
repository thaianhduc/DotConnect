using System;

namespace DotConnect.CommandHandler.Tests
{
    public abstract class DbBaseTest
    {

    }

    public abstract class CommandScenarioTest<TCommand, TResult, TCommandHandler> : DbBaseTest
        where TCommand : class
        where TResult : class
        where TCommandHandler : class
    {
        protected abstract TCommand Arrange();
        protected abstract void Assert(TResult result);
        public void Execute()
        {
            var command = Arrange();

        }
    }
}
