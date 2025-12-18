using CommandBridge.Attributes;
using CommandBridge.Configuration;
using CommandBridge.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using CommandBridge.Extensions;

namespace CommandBridge.Registrars
{
    public class FromAssemblyCommandRegistrar : ICommandRegistrar
    {
        /// <summary>
        /// The assembly to scan
        /// </summary>
        protected readonly Assembly Assembly;

        /// <summary>
        /// The scan options
        /// </summary>
        protected readonly ScanAssemblyOptions Options;

        /// <summary>
        /// All interceptors found in the assembly
        /// </summary>
        protected readonly Lazy<IReadOnlyList<Type>> Interceptors;

        #region Ctor

        public FromAssemblyCommandRegistrar(Assembly assembly, ScanAssemblyOptions options)
        {
            Assembly = assembly;
            Options = options;
            Interceptors = new Lazy<IReadOnlyList<Type>>(() => GetAllInterceptors().ToList());
        }

        #endregion

        #region Register

        public void Register(CommandBridgeConfigurationBuilder builder)
        {
            foreach (var (command, handler) in GetAllCommandAndHandlers())
            {
                var commandConfig = builder.AddCommand(command, handler);
                
                if (Options.Interceptors)
                {
                    //Register command specific interceptors
                    foreach (var interceptor in GetCommandInterceptors(command))
                    {
                        commandConfig.WithInterceptor(interceptor);
                    }
                }
            }

        }

        #endregion

        #region GetAllCommandAndHandlers

        protected virtual IEnumerable<(Type Command, Type Handler)> GetAllCommandAndHandlers()
        {
            var visited = new HashSet<Type>();

            foreach (var type in Assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;

                if (type.GetCustomAttribute<IgnoreAttribute>(inherit: true) != null)
                    continue;

                var commandHandlerInterface = type
                    .GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

                if (commandHandlerInterface is null)
                    continue;

                var genericArgs = commandHandlerInterface.GetGenericArguments();
                var commandType = genericArgs[0];

                if (commandType.IsGenericType && type.IsGenericTypeDefinition)
                    commandType = commandType.GetGenericTypeDefinition();

                if (!visited.Contains(commandType))
                {
                    if (!Options.Filter(commandType, type))
                        continue;

                    visited.Add(commandType);
                    yield return (commandType, type);
                }
                else
                    throw new Exception($"Multiple command handlers found for command {commandType.FullName}. Only one handler is allowed per command.");
            }
        }

        #endregion

        #region GetAllInterceptors

        protected virtual IEnumerable<Type> GetAllInterceptors()
        {
            foreach (var type in Assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;

                if (type.GetCustomAttribute<IgnoreAttribute>(inherit: true) != null)
                    continue;

                var commandInterceptorInterface = type
                    .GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandInterceptor<,>));

                if (commandInterceptorInterface is null)
                    continue;

                yield return type;
            }
        }

        #endregion

        #region GetCommandInterceptors

        protected virtual IEnumerable<Type> GetCommandInterceptors(Type command)
        {
            foreach (var interceptor in Interceptors.Value)
            {
                var interceptorInterface = interceptor.GetRequiredInterceptorInterface();
                var interceptorInterfaceGenericArguments = interceptorInterface.GetGenericArguments();
                var interceptorCommandType = interceptorInterfaceGenericArguments[0];
                var interceptorResultType = interceptorInterfaceGenericArguments[1];

                // Command:     MyCommand : ICommand<int>
                // Interceptor: MyInterceptor : ICommandInterceptor<MyCommand, int>
                if (command == interceptorCommandType) 
                {
                    yield return interceptor;
                    continue;
                }

                if (command.IsGenericTypeDefinition) 
                {
                    if (!interceptorCommandType.IsGenericType)
                        continue;

                    var interceptorCommandTypeDef = interceptorCommandType.GetGenericTypeDefinition();
                   
                    if (interceptorCommandTypeDef == command) 
                    {
                        yield return interceptor;
                        continue;
                    }
                }
            }
        }

        #endregion
    }
}