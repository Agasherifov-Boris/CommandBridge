namespace CommandBridge.Tests.Performance.Models
{
    public class CalculateSumService(IEnumerable<ICalculateSumInterceptor> interceptors)
    {
        public async Task<int> CalculateAsync(CalculateSumCommand command)
        {
            foreach (var interceptor in interceptors)
            {
                var result = await interceptor.CalculatingAsync(command);
                if (result.HasValue)
                {
                    foreach (var interceptor1 in interceptors.Reverse())
                    {
                        await interceptor1.CalculatedAsync(command, result.Value);
                    }

                    return result.Value;
                }
            }

            var calculationResult = command.A + command.B;

            foreach (var interceptor in interceptors.Reverse())
            {
                await interceptor.CalculatedAsync(command, calculationResult);
            }

            return calculationResult;
        }
    }
}