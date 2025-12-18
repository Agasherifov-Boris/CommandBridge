using CommandBridge.Collections;
using CommandBridge.Configuration;
using CommandBridge.Interfaces;
using CommandBridge.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace CommandBridge.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommandBridge(this IServiceCollection services, Action<CommandBridgeConfigurationBuilder> builder)
        {
            var configBuilder = new CommandBridgeConfigurationBuilder(services);

            builder(configBuilder);

            services.Configure<CommandBridgeConfiguration>(_ => { });

            services.TryAddScoped<ICommandSender, CommandSender>();
            services.TryAddSingleton<CommandPipelineCollection>();
            services.TryAddSingleton<ICommandDelegateFactory, CommandDelegateFactory>();
            services.TryAddSingleton<ICommandPipelineProvider, CommandPipelineProvider>();
            services.TryAddSingleton<ICommandInterceptorProvider, CommandInterceptorProvider>();
        }
    }
}