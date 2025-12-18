using CommandBridge.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CommandBridge.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UseInterceptorAttribute : Attribute
    {
        public Type InterceptorType { get; }

        public UseInterceptorAttribute(Type interceptorType)
        {
            ThrowIfInvalidType(interceptorType);
            InterceptorType = interceptorType;
        }

        private static void ThrowIfInvalidType(Type type)
        {
            var isValid = type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandInterceptor<,>));

            if (!isValid)
            {
                throw new ArgumentException($"Type {type} is not a valid interceptor type. It must implement ICommandInterceptor<TCommand, TResult>.");
            }
        }
    }
}