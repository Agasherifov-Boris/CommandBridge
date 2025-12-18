using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using CommandBridge.Extensions;
using CommandBridge.Tests.Performance.Models;
using CommandBridge.Interfaces;
using MediatR;

namespace CommandBridge.Tests.Performance.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class CompareLifetimes
    {
        protected IServiceProvider ServiceProvider = null!;

        [Params(true, false)]
        //[Params(true)]
        public bool IsAsync;

        //[Params(0, 1, 2, 3, 4, 5)]
        [Params(0, 1, 3, 5)]
        //[Params(5)]
        public int InterceptorsCount;

        [Params(ServiceLifetime.Transient, ServiceLifetime.Singleton)]
        public ServiceLifetime Lifetime;

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
                            opts.AddInterceptor(interceptor, lifetime: Lifetime);
                    }

                    opts.AddCommand<CalculateSumCommand, CalculateSumCommandHandler, int>();
                });
            }); 
        }

        #endregion

        #region Benchmarks

        [Benchmark]
        public async Task<int> CommandBridge()
        {
            var command = new CalculateSumCommand(16, 4);
            var commandSender = ServiceProvider.GetRequiredService<ICommandSender>();
            return await commandSender.SendAsync(command);
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