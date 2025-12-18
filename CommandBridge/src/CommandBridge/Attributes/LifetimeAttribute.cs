using Microsoft.Extensions.DependencyInjection;
using System;

namespace CommandBridge.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LifetimeAttribute : Attribute
    {
        public LifetimeAttribute(ServiceLifetime lifetime) 
        {
            Lifetime = lifetime;
        }

        public ServiceLifetime Lifetime { get; }
    }
}