using System.Collections.Generic;

namespace CommandBridge.Tests.Tools.Services
{
    public class Logger
    {
        public List<string> Logs { get; } = new List<string>();

        public void Log(string message)
        {
            Logs.Add(message);
        }
    }
}