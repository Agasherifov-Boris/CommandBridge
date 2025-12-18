namespace CommandBridge.Tests.Performance.Models
{
    public interface ICalculateSumInterceptor
    {
        ValueTask<int?> CalculatingAsync(CalculateSumCommand command);
        ValueTask CalculatedAsync(CalculateSumCommand command, int result);
    }
}