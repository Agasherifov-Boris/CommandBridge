using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using CommandBridge.Extensions;
using CommandBridge.Tests.Performance.Models;
using CommandBridge.Interfaces;
using MediatR;

namespace CommandBridge.Tests.Performance.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class CalculateSum_5
    {
        protected IServiceProvider ServiceProvider = null!;

        #region Setup

        [GlobalSetup]
        public void Setup()
        {
            ServiceProvider = CreateServiceProvider(services =>
            {
                services.AddCommandBridge(opts =>
                {
                    opts.AddInterceptor(typeof(AsyncInterceptor1<,>));
                    opts.AddInterceptor(typeof(AsyncInterceptor2<,>));
                    opts.AddInterceptor(typeof(AsyncInterceptor3<,>));
                    opts.AddInterceptor(typeof(AsyncInterceptor4<,>));
                    opts.AddInterceptor(typeof(AsyncInterceptor5<,>));
                    opts.AddCommand<CalculateSumCommand, CalculateSumCommandHandler, int>();
                });


                services.AddMediatR(opts =>
                {
                    opts.RegisterServicesFromAssemblyContaining<CalculateSumCommand>();
                });
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior1<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior2<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior3<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior4<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior5<,>));


                services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor1>();
                services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor2>();
                services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor3>();
                services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor4>();
                services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor5>();

                services.AddTransient<CalculateSumService>();
            }); 
        }

        #endregion

        #region Benchmarks

        [Benchmark(Baseline = true)]
        public async Task<int> Service()
        {
            var command = new CalculateSumCommand(16, 4);
            var service = ServiceProvider.GetRequiredService<CalculateSumService>();
            return await service.CalculateAsync(command);
        }

        [Benchmark]
        public async Task<int> CommandBridge()
        {
            var command = new CalculateSumCommand(16, 4);
            var commandSender = ServiceProvider.GetRequiredService<ICommandSender>();
            return await commandSender.SendAsync(command);
        }

        [Benchmark]
        public async Task<int> MediatR()
        {
            var request = new CalculateSumRequest(16, 4);
            var mediator = ServiceProvider.GetRequiredService<IMediator>();
            return await mediator.Send(request);
        }

        #endregion

        #region Protected methods

        protected virtual IServiceProvider CreateServiceProvider(Action<ServiceCollection> services)
        {
            var serviceCollection = new ServiceCollection();

            services(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        #endregion
    }
}