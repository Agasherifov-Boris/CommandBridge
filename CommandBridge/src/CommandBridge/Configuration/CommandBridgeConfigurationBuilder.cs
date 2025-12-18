using CommandBridge.Attributes;
using CommandBridge.Extensions;
using CommandBridge.Interfaces;
using CommandBridge.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandBridge.Configuration
{
    public class CommandBridgeConfigurationBuilder
    {
        private readonly IServiceCollection _services;

        #region Ctor

        public CommandBridgeConfigurationBuilder(IServiceCollection services) 
        {
            _services = services;
        }

        #endregion

        #region AddInterceptor

        /// <summary>
        /// Add interceptor to all commands.
        /// </summary>
        /// <typeparam name="TInterceptor"></typeparam>
        /// <returns></returns>
        public CommandBridgeConfigurationBuilder AddInterceptor(Type type, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            if (!type.IsGenericTypeDefinition)
                throw new Exception("Global interceptor must be open generic type");

            type.IsInterceptorEnsure();

            var descriptor = new ServiceDescriptor(type, type, lifetime);
            _services.Add(descriptor);
            _services.Configure<CommandBridgeConfiguration>(config =>
            {
                config.Interceptors.Add(type);
            });

            return this;
        }

        #endregion

        #region AddCommand

        /// <summary>
        /// Add or replace a command handler to a command type.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        public CommandConfigurator<TCommand, TResult> AddCommand<TCommand, THandler, TResult>(ServiceLifetime? lifetime = null)
            where TCommand : ICommand<TResult>
            where THandler : class, ICommandHandler<TCommand, TResult>
        {
            var commandConfig = new CommandConfigurator<TCommand, TResult>(typeof(THandler), _services);

            var commandType = typeof(TCommand);
            var handlerType = typeof(THandler);

            _services.Configure<CommandBridgeConfiguration>(config =>
            {
                config.Commands[commandType] = commandConfig;
            });

            TryAddService(handlerType, lifetime ?? handlerType.GetLifetime());
            AddCommandInterceptors(commandType, handlerType, commandConfig);

            return commandConfig;
        }

        public CommandConfiguration AddCommand(Type commandType, Type handlerType, ServiceLifetime? lifetime = null)
        {
            commandType.IsCommandEnsure();
            handlerType.IsHandlerOfCommandEnsure(commandType);

            var commandConfig = new CommandConfiguration(commandType, handlerType, _services);

            _services.Configure<CommandBridgeConfiguration>(config =>
            {
                config.Commands[commandType] = commandConfig;
            });

            TryAddService(handlerType, lifetime ?? handlerType.GetLifetime());
            AddCommandInterceptors(commandType, handlerType, commandConfig);

            return commandConfig;
        }

        #endregion

        #region Private Methods

        private void TryAddService(Type serviceType, ServiceLifetime lifetime) 
        {
            var descriptor = new ServiceDescriptor(serviceType, serviceType, lifetime);
            _services.TryAdd(descriptor);
        }

        private void AddCommandInterceptors(Type commandType, Type handlerType, CommandConfiguration configurator) 
        {
            var attrs = commandType.GetCustomAttributes<UseInterceptorAttribute>(inherit: true)
                .Concat(handlerType.GetCustomAttributes<UseInterceptorAttribute>(inherit: true));

            foreach (var attr in attrs) 
            {
                var interceptorType = attr.InterceptorType;

                interceptorType.IsInterceptorOfEnsure(commandType);

                configurator.Interceptors.Add(interceptorType);
                TryAddService(interceptorType, interceptorType.GetLifetime());
            }
        }

        #endregion

    }
}