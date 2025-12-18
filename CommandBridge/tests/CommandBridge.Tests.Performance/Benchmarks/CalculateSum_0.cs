using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using CommandBridge.Extensions;
using CommandBridge.Tests.Performance.Models;
using CommandBridge.Interfaces;
using MediatR;

namespace CommandBridge.Tests.Performance.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class CalculateSum_0
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
                    opts.AddCommand<CalculateSumCommand, CalculateSumCommandHandler, int>();
                });

                services.AddMediatR(opts =>
                {
                    opts.RegisterServicesFromAssemblyContaining<CalculateSumCommand>();
                });

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