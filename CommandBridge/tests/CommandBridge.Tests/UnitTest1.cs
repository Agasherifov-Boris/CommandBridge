//using Microsoft.Extensions.DependencyInjection;
//using CommandBridge.Extensions;
//using CommandBridge.Interfaces;
//using CommandBridge.Configuration;
//using CommandBridge.Models;

//namespace CommandBridge.Tests
//{
//    public class UnitTest1
//    {
//        [Fact]
//        public async Task Test1()
//        {
//            var commandSender = CreateCommandSender(cfg =>
//            {
//                cfg.AddInterceptor(typeof(GlobalInterceptor<,>));

//                cfg.AddCommand<DemoCommand, DemoCommandHandler, int>()
//                    .WithInterceptor(async (ctx, next) =>
//                    {
//                        Console.WriteLine("   Start Global Interceptor 2");

//                        var result = await next(ctx);

//                        Console.WriteLine("   End Global Interceptor 2");

//                        return result;
//                    });
//            });

//            var command = new DemoCommand();
//            await commandSender.SendAsync(command);

//        }

//        private ICommandSender CreateCommandSender(Action<CommandBridgeConfigurationBuilder> cfg) 
//        {
//            var services = new ServiceCollection();

//            services.AddCommandBridge(cfg);

//            var serviceProvider = services.BuildServiceProvider();

//            return serviceProvider.GetRequiredService<ICommandSender>();
//        }

//        public class DemoCommand : ICommand<int>
//        {
            
//        }

//        public class DemoCommandHandler : ICommandHandler<DemoCommand, int>
//        {
//            public ValueTask<int> HandleAsync(DemoCommand command, CancellationToken ct)
//            {
//                Console.WriteLine("Handle command");
//                return ValueTask.FromResult(1);
//            }
//        }
//    }

//    public class GlobalInterceptor<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
//        where TCommand : ICommand<TResult>
//    {
//        public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
//        {
//            Console.WriteLine("GlobalInterceptor start");

//            var result = await next(context);

//            Console.WriteLine("GlobalInterceptor finished");

//            return result;
//        }
//    }
//}