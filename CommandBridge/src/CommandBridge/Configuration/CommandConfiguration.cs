using CommandBridge.Extensions;
using CommandBridge.Interceptors;
using CommandBridge.Interfaces;
using CommandBridge.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandBridge.Configuration
{

    #region CommandConfigurator

    public class CommandConfiguration
    {
        protected IServiceCollection Services { get; }

        internal Type CommandType { get; }
        internal Type HandlerType { get; }
        internal List<object> Interceptors { get; } = new List<object>();

        #region Ctor

        public CommandConfiguration(Type commandType, Type handlerType, IServiceCollection services)
        {
            handlerType.IsHandlerOfCommandEnsure(commandType);

            CommandType = commandType;
            HandlerType = handlerType;
            Services = services;
        }

        #endregion

        public CommandConfiguration WithInterceptor(Type interceptorType) 
        {
            var serviceType = interceptorType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandInterceptor<,>));

            if (serviceType == null)
                throw new Exception($"The type {interceptorType.FullName} is not a valid command interceptor. It must implement ICommandInterceptor<TCommand, TResult> interface.");

            var descriptor = new ServiceDescriptor(interceptorType, interceptorType, interceptorType.GetLifetime());
            Services.TryAdd(descriptor);

            Interceptors.Add(interceptorType);

            return this;
        }
    }

    #endregion

    #region CommandConfiguration<TCommand, TResult>

    public class CommandConfigurator<TCommand, TResult> : CommandConfiguration
        where TCommand : ICommand<TResult>
    {
        #region Ctor

        public CommandConfigurator(Type handlerType, IServiceCollection services)
            :base(typeof(TCommand), handlerType, services)
        {
        }

        #endregion

        public CommandConfigurator<TCommand, TResult> WithInterceptor(Func<ICommandContext<TCommand, TResult>, CommandDelegate<TCommand, TResult>, ValueTask<TResult>> interceptor)
        {
            var interceptorImpl = new DelegatedInterceptor<TCommand, TResult>(interceptor);
            Interceptors.Add(interceptorImpl);
            return this;
        }

        public CommandConfigurator<TCommand, TResult> WithInterceptor<TInterceptor>(ServiceLifetime? lifetime = null)
            where TInterceptor : ICommandInterceptor<TCommand, TResult>
        {
            if (lifetime == null)
                lifetime = typeof(TInterceptor).GetLifetime();

            var descriptor = ServiceDescriptor.Describe(typeof(TInterceptor), typeof(TInterceptor), lifetime.Value);
            Services.TryAdd(descriptor);
            Interceptors.Add(typeof(TInterceptor));

            return this;
        }

        public CommandConfigurator<TCommand, TResult> WithInterceptor<TInterceptor>(TInterceptor interceptor)
            where TInterceptor : ICommandInterceptor<TCommand, TResult>
        {
            Interceptors.Add(interceptor);
            return this;
        }
    }

    #endregion

}