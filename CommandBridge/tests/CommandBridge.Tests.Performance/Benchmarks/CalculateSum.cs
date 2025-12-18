using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using CommandBridge.Extensions;
using CommandBridge.Tests.Performance.Models;
using CommandBridge.Interfaces;
using MediatR;

namespace CommandBridge.Tests.Performance.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class CalculateSum
    {
        protected IServiceProvider ServiceProvider = null!;

        [Params(true, false)]
        //[Params(true)]
        public bool IsAsync;

        //[Params(0, 1, 2, 3, 4, 5)]
        [Params(0, 1, 5)]
        //[Params(5)]
        public int InterceptorsCount;

        #region Setup

        [GlobalSetup]
        public void Setup()
        {
            ServiceProvider = CreateServiceProvider(services =>
            {
                services.AddCommandBridge(opts =>
                {
                    if (InterceptorsCount > 0) 
                    { 
                        var interceptors = new List<Type>();

                        if (InterceptorsCount >= 1)
                            interceptors.Add(IsAsync ? typeof(AsyncInterceptor1<,>) : typeof(SyncInterceptor1<,>));
                        if (InterceptorsCount >= 2)
                            interceptors.Add(IsAsync ? typeof(AsyncInterceptor2<,>) : typeof(SyncInterceptor2<,>));
                        if (InterceptorsCount >= 3)
                            interceptors.Add(IsAsync ? typeof(AsyncInterceptor3<,>) : typeof(SyncInterceptor3<,>));
                        if (InterceptorsCount >= 4)
                            interceptors.Add(IsAsync ? typeof(AsyncInterceptor4<,>) : typeof(SyncInterceptor4<,>));
                        if (InterceptorsCount >= 5)
                            interceptors.Add(IsAsync ? typeof(AsyncInterceptor5<,>) : typeof(SyncInterceptor5<,>));

                        foreach (var interceptor in interceptors)
                            opts.AddInterceptor(interceptor);
                    }

                    opts.AddCommand<CalculateSumCommand, CalculateSumCommandHandler, int>();
                });

                services.AddMediatR(opts =>
                {
                    opts.RegisterServicesFromAssemblyContaining<CalculateSumCommand>();
                });

                services.AddTransient<CalculateSumService>();

                if (InterceptorsCount > 0)
                {
                    var pipelineBehaviors = new List<Type>();
                    var calcSumInterceptors = new List<Type>();

                    if (InterceptorsCount >= 1) 
                    {
                        calcSumInterceptors.Add(IsAsync ? typeof(AsyncCalculateSumInterceptor1) : typeof(SyncCalculateSumInterceptor1));
                        pipelineBehaviors.Add(IsAsync ? typeof(AsyncPipelineBehavior1<,>) : typeof(SyncPipelineBehavior1<,>));
                    }
                        
                    if (InterceptorsCount >= 2)
                    {
                        calcSumInterceptors.Add(IsAsync ? typeof(AsyncCalculateSumInterceptor2) : typeof(SyncCalculateSumInterceptor2));
                        pipelineBehaviors.Add(IsAsync ? typeof(AsyncPipelineBehavior2<,>) : typeof(SyncPipelineBehavior2<,>));
                    }

                    if (InterceptorsCount >= 3)
                    {
                        calcSumInterceptors.Add(IsAsync ? typeof(AsyncCalculateSumInterceptor3) : typeof(SyncCalculateSumInterceptor3));
                        pipelineBehaviors.Add(IsAsync ? typeof(AsyncPipelineBehavior3<,>) : typeof(SyncPipelineBehavior3<,>));
                    }

                    if (InterceptorsCount >= 4)
                    {
                        calcSumInterceptors.Add(IsAsync ? typeof(AsyncCalculateSumInterceptor4) : typeof(SyncCalculateSumInterceptor4));
                        pipelineBehaviors.Add(IsAsync ? typeof(AsyncPipelineBehavior4<,>) : typeof(SyncPipelineBehavior4<,>));
                    }

                    if (InterceptorsCount >= 5)
                    {
                        calcSumInterceptors.Add(IsAsync ? typeof(AsyncCalculateSumInterceptor5) : typeof(SyncCalculateSumInterceptor5));
                        pipelineBehaviors.Add(IsAsync ? typeof(AsyncPipelineBehavior5<,>) : typeof(SyncPipelineBehavior5<,>));
                    }

                    foreach (var pipelineBehavior in pipelineBehaviors)
                        services.AddTransient(typeof(IPipelineBehavior<,>), pipelineBehavior);

                    foreach (var calcSumInterceptor in calcSumInterceptors)
                        services.AddTransient(typeof(ICalculateSumInterceptor), calcSumInterceptor);
                }

                
            }); 
        }

        #endregion

        #region Benchmarks

        [Benchmark]
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
        public async Task<int> Mediator()
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