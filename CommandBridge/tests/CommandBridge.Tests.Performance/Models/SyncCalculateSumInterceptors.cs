
namespace CommandBridge.Tests.Performance.Models
{
    public class SyncCalculateSumInterceptor1 : ICalculateSumInterceptor
    {
        public ValueTask<int?> CalculatingAsync(CalculateSumCommand command) => ValueTask.FromResult((int?)null);
        public ValueTask CalculatedAsync(CalculateSumCommand command, int result) => ValueTask.CompletedTask;
    }

    public class SyncCalculateSumInterceptor2 : ICalculateSumInterceptor
    {
        public ValueTask<int?> CalculatingAsync(CalculateSumCommand command) => ValueTask.FromResult((int?)null);
        public ValueTask CalculatedAsync(CalculateSumCommand command, int result) => ValueTask.CompletedTask;
    }

    public class SyncCalculateSumInterceptor3 : ICalculateSumInterceptor
    {
        public ValueTask<int?> CalculatingAsync(CalculateSumCommand command) => ValueTask.FromResult((int?)null);
        public ValueTask CalculatedAsync(CalculateSumCommand command, int result) => ValueTask.CompletedTask;
    }

    public class SyncCalculateSumInterceptor4 : ICalculateSumInterceptor
    {
        public ValueTask<int?> CalculatingAsync(CalculateSumCommand command) => ValueTask.FromResult((int?)null);
        public ValueTask CalculatedAsync(CalculateSumCommand command, int result) => ValueTask.CompletedTask;
    }

    public class SyncCalculateSumInterceptor5 : ICalculateSumInterceptor
    {
        public ValueTask<int?> CalculatingAsync(CalculateSumCommand command) => ValueTask.FromResult((int?)null);
        public ValueTask CalculatedAsync(CalculateSumCommand command, int result) => ValueTask.CompletedTask;
    }
}