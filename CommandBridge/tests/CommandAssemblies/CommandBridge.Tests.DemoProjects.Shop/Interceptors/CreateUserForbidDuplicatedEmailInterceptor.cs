using CommandBridge.Interfaces;
using CommandBridge.Models;
using CommandBridge.Tests.DemoProjects.Shop.Commands;
using CommandBridge.Tests.DemoProjects.Shop.Entities;
using CommandBridge.Tests.DemoProjects.Shop.Exceptions;

namespace CommandBridge.Tests.DemoProjects.Shop.Interceptors
{
    public class CreateUserForbidDuplicatedEmailInterceptor : ICommandInterceptor<CreateEntityCommand<User>, User>
    {
        public ValueTask<User> HandleAsync(ICommandContext<CreateEntityCommand<User>, User> context, CommandDelegate<CreateEntityCommand<User>, User> next)
        {
            if (context.Command.Entity.Email == "duplicate@outlook.com")
                throw new DuplicateEmailException();

            return next(context);
        }
    }
}