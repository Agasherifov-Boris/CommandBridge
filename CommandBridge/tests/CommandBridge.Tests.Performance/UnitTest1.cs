using BenchmarkDotNet.Running;
using CommandBridge.Tests.Performance.Benchmarks;
using System.Threading.Tasks;

namespace CommandBridge.Tests.Performance
{
    public class Tests
    {
        [Test]
        public async Task Test1()
        {
            //var benchmark = new CalculateSum_5();
            //benchmark.Setup();
            //var res1 = await benchmark.CommandBridge();
            //var res2 = await benchmark.Service();
            //var res3 = await benchmark.MediatR();

            BenchmarkRunner.Run<CalculateSum_5>();
        }
    }
}