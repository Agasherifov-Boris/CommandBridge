using System;

namespace CommandBridge.Interfaces
{
    public interface ICommandInterceptorProvider
    {
        /// <summary>
        /// Gets the command interceptors for the specified interceptor type.
        /// </summary>
        /// <param name="interceptorType"></param>
        /// <returns></returns>
        object[] GetInterceptors(Type interceptorType);
    }
}