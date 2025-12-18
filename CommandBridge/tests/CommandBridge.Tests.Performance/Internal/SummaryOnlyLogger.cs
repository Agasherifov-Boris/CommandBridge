using BenchmarkDotNet.Loggers;

namespace CommandBridge.Tests.Performance.Internal
{
    internal class SummaryOnlyLogger : ILogger
    {
        public string Id => "SummaryOnly";
        public int Priority => 0;
        public void Write(LogKind logKind, string text) { }
        public void WriteLine() { }
        public void WriteLine(LogKind logKind, string text)
        {
            if (logKind == LogKind.Result || logKind == LogKind.Statistic || logKind == LogKind.Help)
                Console.WriteLine(text);
        }
        public void Flush() { }
    }
}