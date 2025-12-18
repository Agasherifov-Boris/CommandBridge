using CommandBridge.Attributes;
using CommandBridge.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace CommandBridge.Extensions
{
    internal static class TypeExtensions
    {
        #region GetLifetime

        public static ServiceLifetime GetLifetime(this Type type)
        {
            var attr = type.GetCustomAttribute<LifetimeAttribute>(inherit: true);
            return attr?.Lifetime ?? ServiceLifetime.Transient;
        }

        #endregion

        #region IsHandlerOfCommand

        public static bool IsHandlerOfCommand(this Type handlerType, Type commandType)
        {
            if (!commandType.IsCommand())
                return false;

            if (!handlerType.IsCommandHandler())
                return false;

            var expectingCommandType = handlerType.GetRequiredCommandHandlerInterface().GetGenericArguments()[0];

            if (commandType.IsGenericTypeDefinition && expectingCommandType.IsGenericType) 
            {
                expectingCommandType = expectingCommandType.GetGenericTypeDefinition();
            }
            
            return expectingCommandType == commandType;
        }

        public static void IsHandlerOfCommandEnsure(this Type handlerType, Type commandType) 
        { 
            if (!handlerType.IsHandlerOfCommand(commandType))
                throw new Exception($"The type {handlerType.FullName} is not a handler of command {commandType.FullName}");
        }

        #endregion

        #region GetCommandInterface

        public static Type? GetCommandInterface(this Type type) 
        {
            return type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>));
        }

        public static Type GetRequiredCommandInterface(this Type type)
        {
            var result = type.GetCommandInterface();
            if (result == null)
                throw new Exception($"The type {type.FullName} is not a valid command. It must implement ICommand<TResult> interface.");
            return result;
        }

        #endregion

        #region GetInterceptorInterface

        public static Type? GetInterceptorInterface(this Type type)
        {
            return type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandInterceptor<,>));
        }

        public static Type GetRequiredInterceptorInterface(this Type type)
        {
            var result = type.GetInterceptorInterface();
            if (result == null)
                throw new Exception($"The type {type.FullName} is not a valid interceptor. It must implement ICommandInterceptor<TCommand, TResult> interface.");
            return result;
        }

        #endregion

        #region IsCommand

        public static bool IsCommand(this Type type)
        {
            return type.GetCommandInterface() != null;
        }

        public static void IsCommandEnsure(this Type type)
        {
            if (!type.IsCommand())
                throw new Exception($"The type {type.FullName} is not a valid command. It must implement ICommand<TResult> interface.");
        }

        #endregion

        #region GetCommandResultType

        public static Type GetCommandResultType(this Type commandType) 
        {
            commandType.IsCommandEnsure();
            return commandType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>))
                .GetGenericArguments()[0];
        }

        #endregion

        #region IsCommandHandler

        public static bool IsCommandHandler(this Type type)
        {
            return type.GetCommandHandlerInterface() != null;
        }

        #endregion

        #region GetCommandHandlerInterface

        public static Type? GetCommandHandlerInterface(this Type type) 
        {
            return type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
        }

        public static Type GetRequiredCommandHandlerInterface(this Type type) 
        { 
            var result = type.GetCommandHandlerInterface();

            if (result == null)
                throw new Exception($"The type {type.FullName} is not a valid command handler. It must implement ICommandHandler<TCommand, TResult> interface.");

            return result;
        }

        #endregion

        #region Interceptor

        public static bool IsInterceptor(this Type type)
        {
            return type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandInterceptor<,>));
        }

        public static void IsInterceptorEnsure(this Type type)
        {
            if (!type.IsInterceptor())
                throw new Exception($"The type {type.FullName} is not an interceptor. It must implement ICommandInterceptor interface.");
        }

        public static bool IsInterceptorOf(this Type interceptor, Type command) 
        {
            if (interceptor == null || command == null) return false;

            var interceptorInterface = interceptor.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandInterceptor<,>));
            if (interceptorInterface == null) return false;

            var commandInterface = command.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>));
            if (commandInterface == null) return false;

            var expectedCommandType = interceptorInterface.GetGenericArguments()[0];
            var expectedResultType = interceptorInterface.GetGenericArguments()[1];

            var commandResultType = commandInterface.GetGenericArguments()[0];

            bool resultMatches = expectedResultType.IsGenericParameter || expectedResultType == commandResultType;

            bool commandMatches;
            if (expectedCommandType.IsGenericParameter)
            {
                commandMatches = true;
            }
            else if (expectedCommandType == command)
            {
                commandMatches = true;
            }
            else if (expectedCommandType.IsGenericType)
            {
                var expectedDef = expectedCommandType.GetGenericTypeDefinition();

                if (command.IsGenericTypeDefinition)
                {
                    commandMatches = expectedDef == command;
                }
                else if (command.IsGenericType)
                {
                    commandMatches = expectedDef == command.GetGenericTypeDefinition();
                }
                else
                {
                    commandMatches = false;
                }
            }
            else if (expectedCommandType.IsGenericTypeDefinition)
            {
                commandMatches = command.IsGenericType
                    ? command.GetGenericTypeDefinition() == expectedCommandType
                    : command == expectedCommandType;
            }
            else
            {
                commandMatches = false;
            }

            return commandMatches && resultMatches;
        }

        public static void IsInterceptorOfEnsure(this Type interceptor, Type command)
        {
            if (!interceptor.IsInterceptorOf(command))
                throw new Exception($"The type {interceptor.FullName} is not an interceptor of command {command.FullName}");
        }

        #endregion

    }
}