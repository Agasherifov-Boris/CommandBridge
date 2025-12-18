using System;

namespace CommandBridge.Configuration
{
    public class ScanAssemblyOptions
    {
        /// <summary>
        /// Filter to exclude command-handler pairs. Return true to include the pair, false to exclude it.
        /// </summary>
        public ScanAssemblyFilter Filter { get; set; } = (c, h) => true;

        /// <summary>
        /// Find interceptors for each command in the assembly, by default true
        /// </summary>
        public bool Interceptors { get; set; } = true;

        public static ScanAssemblyOptions Default => new ScanAssemblyOptions();
    }

    public delegate bool ScanAssemblyFilter(Type commandType, Type handlerType);
}