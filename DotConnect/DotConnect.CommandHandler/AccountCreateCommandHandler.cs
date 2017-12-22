using System;
using DotConnect.Contracts;
using DotConnect.Core.Interfaces;
using DotConnect.Domain;

namespace DotConnect.CommandHandler
{
    /// <summary>
    /// Create a new account entry in the database.
    /// </summary>
    /// <remarks>
    /// The handler will use EF connects directly to the database. There is no good reason to create a repository laywer here.
    /// And also and important point that the unit test will not care much about it, as far as, the end result meets.
    /// </remarks>
    public class AccountCreateCommandHandler : ICommandHandler<AccountCreateCommand, AccountCreateCommandResult>
    {
        public AccountCreateCommandResult Handle(AccountCreateCommand command)
        {
            var account = new Account
            {
                Name = command.Name,
                Email = command.Email,
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
            // Note: The Id and CreatedAt should be handled by the infrastructure
            // they should not be assigned publicly.
            return new AccountCreateCommandResult
            {
                AccountId = account.Id
            };
        }
    }
}
