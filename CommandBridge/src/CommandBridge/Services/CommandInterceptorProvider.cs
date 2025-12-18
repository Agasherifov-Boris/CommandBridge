using CommandBridge.Extensions;
using CommandBridge.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CommandBridge.Services
{
    public class CommandInterceptorProvider : ICommandInterceptorProvider
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CommandInterceptorProvider(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public object[] GetInterceptors(Type interceptorType)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            return scope.ServiceProvider.GetServices(interceptorType)
                .WithoutNulls()
                .ToArray();
        }
    }
}