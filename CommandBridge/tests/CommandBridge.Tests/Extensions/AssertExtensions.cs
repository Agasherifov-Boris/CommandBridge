using CommandBridge.Tests.Tools.Services;

namespace CommandBridge.Tests.Extensions
{
    public static class AssertExtensions
    {
        public static void ShouldBeEmpty(this Logger logger) => ShouldBe(logger, []); 
        public static void ShouldBe(this Logger logger, string[] expected) 
        {
            var actualLogs = logger.Logs;
            Assert.Equal(expected.Length, actualLogs.Count);

            for (int i = 0; i < actualLogs.Count; i++)
                Assert.Equal(expected[i], actualLogs[i]);
        }
    }
}