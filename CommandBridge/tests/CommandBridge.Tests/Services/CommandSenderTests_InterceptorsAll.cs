using CommandBridge.Interfaces;
using CommandBridge.Models;
using CommandBridge.Attributes;
using CommandBridge.Tests.Extensions;
using CommandBridge.Tests.Internal.Tracking;

namespace CommandBridge.Tests.Services
{
    public class CommandSenderTests_InterceptorsAll : CommandSenderTestsBase
    {
       
        [Fact]
        public async Task SendAsync_WithFluentGlobalAttrInterceptors_ValidOrder()
        {
            var container = CreateContainer(opts =>
            {
                opts.AddInterceptor(typeof(GenericCommandInterceptor<,>));

                opts.AddCommand<TestCommand, TestCommandHandler, int>()
                    .WithInterceptor<TestCommandInterceptor3>();
            });

            var command = new TestCommand();

            var result = await container.CommandSender.SendAsync(command);

            Assert.Equal(42, result);

            container.TrackingService.InvokationOrderOf<GenericCommandInterceptor<TestCommand, int>>("HandleAsync").ShouldBe([1, 3]);
            container.TrackingService.InvokationOrderOf<TestCommandInterceptor1>("HandleAsync").ShouldBe([2]);
            container.TrackingService.InvokationOrderOf<TestCommandInterceptor2>("HandleAsync").ShouldBeEmpty();
            container.TrackingService.InvokationOrderOf<TestCommandInterceptor3>("HandleAsync").ShouldBe([4]);
            container.TrackingService.InvokationOrderOf<TestCommandHandler>("HandleAsync").ShouldBe([5]);
        }


        #region TestCommand


        [UseInterceptor(typeof(TestCommandInterceptor1))]
        [UseInterceptor(typeof(GenericCommandInterceptor<,>))]
        public class TestCommand : ICommand<int>
        {
        }

        public class TestCommandHandler : ICommandHandler<TestCommand, int>
        {
            private readonly TrackingService<TestCommandHandler> _trackingService;
            public TestCommandHandler(TrackingService<TestCommandHandler> trackingService)
            {
                trackingService.TrackInvokationOrder("Ctor");
                _trackingService = trackingService;
            }

            public ValueTask<int> HandleAsync(TestCommand command, CancellationToken ct)
            {
                _trackingService.TrackInvokationOrder("HandleAsync");

                return ValueTask.FromResult(42);
            }
        }


        public class TestCommandInterceptor1(TrackingService<TestCommandInterceptor1> trackingService)
            : ICommandInterceptor<TestCommand, int>
        {
            public ValueTask<int> HandleAsync(ICommandContext<TestCommand, int> context, CommandDelegate<TestCommand, int> next)
            {
                trackingService.TrackInvokationOrder("HandleAsync");

                return next(context);
            }
        }

        public class TestCommandInterceptor2(TrackingService<TestCommandInterceptor2> trackingService)
            : ICommandInterceptor<TestCommand, int>
        {
            public ValueTask<int> HandleAsync(ICommandContext<TestCommand, int> context, CommandDelegate<TestCommand, int> next)
            {
                trackingService.TrackInvokationOrder("HandleAsync");

                return next(context);
            }
        }

        public class TestCommandInterceptor3(TrackingService<TestCommandInterceptor3> trackingService)
            : ICommandInterceptor<TestCommand, int>
        {
            public ValueTask<int> HandleAsync(ICommandContext<TestCommand, int> context, CommandDelegate<TestCommand, int> next)
            {
                trackingService.TrackInvokationOrder("HandleAsync");

                return next(context);
            }
        }

        public class TerminateTestCommandInterceptor(TrackingService<TerminateTestCommandInterceptor> trackingService)
            : ICommandInterceptor<TestCommand, int>
        {
            public ValueTask<int> HandleAsync(ICommandContext<TestCommand, int> context, CommandDelegate<TestCommand, int> next)
            {
                trackingService.TrackInvokationOrder("HandleAsync");

                return ValueTask.FromResult(-1);
            }
        }

        public class GenericCommandInterceptor<TCommand, TResult>(TrackingService<GenericCommandInterceptor<TCommand, TResult>> trackingService)
            : ICommandInterceptor<TCommand, TResult>
            where TCommand : ICommand<TResult>
        {
            public ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
            {
                trackingService.TrackInvokationOrder("HandleAsync");

                return next(context);
            }
        }


        #endregion
    }
}