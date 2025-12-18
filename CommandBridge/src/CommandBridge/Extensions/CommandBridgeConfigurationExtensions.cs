using CommandBridge.Configuration;
using CommandBridge.Interfaces;
using CommandBridge.Registrars;
using System;
using System.Linq;
using System.Reflection;

namespace CommandBridge.Extensions
{
    public static class CommandBridgeConfigurationExtensions
    {

        #region From

        public static CommandBridgeConfigurationBuilder FromAssemblies(this CommandBridgeConfigurationBuilder builder, params Assembly[] assemblies) 
        { 
            return FromAssemblies(builder, ScanAssemblyOptions.Default, assemblies);
        }

        public static CommandBridgeConfigurationBuilder FromAssemblies(this CommandBridgeConfigurationBuilder builder, ScanAssemblyOptions options, params Assembly[] assemblies)
        {
            return FromAssembliesCore(builder, assemblies, options);
        }

        public static CommandBridgeConfigurationBuilder FromAssembliesOf(this CommandBridgeConfigurationBuilder builder, params Type[] types)
        {
            return FromAssembliesOf(builder, ScanAssemblyOptions.Default, types);
        }

        public static CommandBridgeConfigurationBuilder FromAssembliesOf(this CommandBridgeConfigurationBuilder builder, ScanAssemblyOptions options, params Type[] types)
        {
            return FromAssemblies(builder, options, types.Select(t => t.Assembly).Distinct().ToArray());
        }

        public static CommandBridgeConfigurationBuilder FromAssemblyOf<T>(this CommandBridgeConfigurationBuilder builder) 
        { 
            return FromAssemblyOf<T>(builder, ScanAssemblyOptions.Default);
        }

        public static CommandBridgeConfigurationBuilder FromAssemblyOf<T>(this CommandBridgeConfigurationBuilder builder, ScanAssemblyOptions options) 
        {
            return FromAssemblies(builder, options, typeof(T).Assembly);
        }

        public static CommandBridgeConfigurationBuilder FromAssemblyContaining<T>(this CommandBridgeConfigurationBuilder builder, Action<ScanAssemblyOptions> options)
        {
            var scanOptions = ScanAssemblyOptions.Default;
            options(scanOptions);
            return FromAssemblies(builder, scanOptions, typeof(T).Assembly);
        }

        private static CommandBridgeConfigurationBuilder FromAssembliesCore(CommandBridgeConfigurationBuilder builder, Assembly[] assemblies, ScanAssemblyOptions options) 
        {
            foreach (var assembly in assemblies)
            {
                var configurator = new FromAssemblyCommandRegistrar(assembly, options);
                builder.From(configurator);
            }

            return builder;
        }

        public static CommandBridgeConfigurationBuilder From<T>(this CommandBridgeConfigurationBuilder builder, T configurator)
            where T : ICommandRegistrar
        {
            configurator.Register(builder);
            return builder;
        }

        #endregion

    }
}