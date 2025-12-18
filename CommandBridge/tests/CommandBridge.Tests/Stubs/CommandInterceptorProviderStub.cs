using CommandBridge.Interfaces;

namespace CommandBridge.Tests.Stubs
{
    public class CommandInterceptorProviderStub : ICommandInterceptorProvider
    {
        private object[] _interceptors;

        public CommandInterceptorProviderStub(IEnumerable<object> interceptors)
        {
            _interceptors = interceptors.ToArray();
        }

        public object[] GetInterceptors(Type interceptorType)
        {
            return _interceptors;
        }

        public void SetInterceptors(object[] interceptors) => _interceptors = interceptors;
    }
}