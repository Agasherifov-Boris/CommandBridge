using CommandBridge.Configuration;
using System;

namespace CommandBridge.Extensions
{
    public static class CommandConfigurationCollectionExtensions
    {
        internal static CommandConfiguration Of(this CommandConfigurationCollection configurations, Type commandType) 
        { 
            if (configurations.TryGetValue(commandType, out var config))
                return config;

            if (commandType.IsGenericType)
            {
                var openGenericType = commandType.GetGenericTypeDefinition();
                if (configurations.TryGetValue(openGenericType, out var openGenericConfig))
                    return openGenericConfig;
            }

            throw new Exception($"No command configuration found for command type {commandType.FullName}");
        }
    }
}