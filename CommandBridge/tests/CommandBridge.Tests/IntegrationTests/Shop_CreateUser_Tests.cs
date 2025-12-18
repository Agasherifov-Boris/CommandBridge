using CommandBridge.Tests.Internal;
using CommandBridge.Extensions;
using CommandBridge.Tests.DemoProjects.Shop.Commands;
using CommandBridge.Tests.DemoProjects.Shop.Entities;
using Microsoft.Extensions.DependencyInjection;
using CommandBridge.Tests.DemoProjects.Shop.Repositories;
using CommandBridge.Tests.DemoProjects.Shop.Interceptors;
using CommandBridge.Tests.DemoProjects.Shop.Exceptions;
using CommandBridge.Tests.Extensions;
using System.ComponentModel.DataAnnotations;
using CommandBridge.Configuration;
using CommandBridge.Tests.DemoProjects.Shop.CommandHandlers;

namespace CommandBridge.Tests.IntegrationTests
{

    public abstract class Shop_CreateUser_Tests
    {

        [Fact]
        public async Task CreateUser_Ok()
        {
            var container = CreateContainer();
            var command = new CreateEntityCommand<User>()
            {
                Entity = new()
                {
                    Name = "John Doe",
                    Email = "john.doe@outlook.com"
                }
            };

            var user = await container.CommandSender.SendAsync(command);

            Assert.NotNull(user);
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("john.doe@outlook.com", user.Email);

            container.Logger.ShouldBe(
            [
                "Starting execution of command CreateEntityCommand`1",
                "Successfully executed command CreateEntityCommand`1"
            ]);
        }

        [Fact]
        public async Task CreateUser_InvalidEmail_Error()
        {
            var container = CreateContainer();
            var command = new CreateEntityCommand<User>()
            {
                Entity = new()
                {
                    Name = "John Doe",
                    Email = "johndoe"
                }
            };

            await Assert.ThrowsAsync<ValidationException>(async () => await container.CommandSender.SendAsync(command));

            container.Logger.ShouldBe(
            [
                "Starting execution of command CreateEntityCommand`1",
                "Error executing command CreateEntityCommand`1: The Email field is not a valid e-mail address."
            ]);
        }

        [Fact]
        public async Task CreateUser_ForbiddenEmail_Error()
        {
            var container = CreateContainer();
            var command = new CreateEntityCommand<User>()
            {
                Entity = new()
                {
                    Name = "John Doe",
                    Email = "johndoe@gmail.com"
                }
            };

            await Assert.ThrowsAsync<ForbiddenEmailBoxException>(async () => await container.CommandSender.SendAsync(command));

            container.Logger.ShouldBe(
            [
                "Starting execution of command CreateEntityCommand`1",
                "Error executing command CreateEntityCommand`1: Email 'GMail' forbidden"
            ]);
        }

        [Fact]
        public async Task CreateUser_DuplicatedEmail_Error()
        {
            var container = CreateContainer();
            var command = new CreateEntityCommand<User>()
            {
                Entity = new()
                {
                    Name = "John Doe",
                    Email = "duplicate@outlook.com"
                }
            };

            await Assert.ThrowsAsync<DuplicateEmailException>(async () => await container.CommandSender.SendAsync(command));

            container.Logger.ShouldBe(
            [
                "Starting execution of command CreateEntityCommand`1",
                "Error executing command CreateEntityCommand`1: User with the same email already exists"
            ]);
        }

        protected abstract void Setup(CommandBridgeConfigurationBuilder cfg);

        private TestContainer CreateContainer()
        {
            return TestContainer.Create(builder =>
            {
                builder.Services.AddCommandBridge(Setup);
                builder.Services.AddScoped<ClientRepository>();
                builder.Services.AddScoped<UserRepository>();
            });
        }
    }

    public class Shop_CreateUser_AutoConfig_Tests : Shop_CreateUser_Tests
    {
        protected override void Setup(CommandBridgeConfigurationBuilder cfg)
        {
            cfg.AddInterceptor(typeof(LoggingInterceptor<,>));
            cfg.AddInterceptor(typeof(ValidationInterceptor<,>));
            cfg.FromAssemblyOf<DemoProjects.Shop.Program>();
        }
    }

    public class Shop_CreateUser_ManualConfig_Tests : Shop_CreateUser_Tests
    {
        protected override void Setup(CommandBridgeConfigurationBuilder cfg)
        {
            cfg.AddInterceptor(typeof(LoggingInterceptor<,>));
            cfg.AddInterceptor(typeof(ValidationInterceptor<,>));

            cfg.AddCommand<CreateEntityCommand<User>, CreateUserCommandHandler, User>()
                .WithInterceptor<CreateUserForbidDuplicatedEmailInterceptor>()
                //.WithInterceptor<CreateUserForbidGmailInterceptor>()
                .WithInterceptor((ctx, next) =>
                {
                    if (ctx.Command.Entity.Email.EndsWith("@gmail.com"))
                        throw new ForbiddenEmailBoxException("GMail");

                    return next(ctx);
                });
        }
    }
}