using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CommandBridge.Configuration
{
    public class CommandBridgeConfiguration
    {
        internal InterceptorCollection Interceptors { get; } = new InterceptorCollection();

        internal CommandConfigurationCollection Commands { get; } = new CommandConfigurationCollection();
    }

    public class InterceptorCollection : List<object>
    { 
        
    }

    public class CommandConfigurationCollection : ConcurrentDictionary<Type, CommandConfiguration> 
    {

    }
}