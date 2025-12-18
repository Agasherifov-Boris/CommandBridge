
namespace CommandBridge.Tests.Performance.Models
{
    public class AsyncCalculateSumInterceptor1 : ICalculateSumInterceptor
    {
        public async ValueTask<int?> CalculatingAsync(CalculateSumCommand command)
        {
            return null;
        }

        public async ValueTask CalculatedAsync(CalculateSumCommand command, int result)
        {
            
        }
    }
    public class AsyncCalculateSumInterceptor2 : ICalculateSumInterceptor
    {
        public async ValueTask<int?> CalculatingAsync(CalculateSumCommand command)
        {
            return null;
        }

        public async ValueTask CalculatedAsync(CalculateSumCommand command, int result)
        {

        }
    }
    public class AsyncCalculateSumInterceptor3 : ICalculateSumInterceptor
    {
        public async ValueTask<int?> CalculatingAsync(CalculateSumCommand command)
        {
            return null;
        }

        public async ValueTask CalculatedAsync(CalculateSumCommand command, int result)
        {

        }
    }
    public class AsyncCalculateSumInterceptor4 : ICalculateSumInterceptor
    {
        public async ValueTask<int?> CalculatingAsync(CalculateSumCommand command)
        {
            return null;
        }

        public async ValueTask CalculatedAsync(CalculateSumCommand command, int result)
        {

        }
    }
    public class AsyncCalculateSumInterceptor5 : ICalculateSumInterceptor
    {
        public async ValueTask<int?> CalculatingAsync(CalculateSumCommand command)
        {
            return null;
        }

        public async ValueTask CalculatedAsync(CalculateSumCommand command, int result)
        {

        }
    }
}