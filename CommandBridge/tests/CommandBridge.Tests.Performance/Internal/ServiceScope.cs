using CommandBridge.Interfaces;
using CommandBridge.Tests.Performance.Models;
using MediatR;

namespace CommandBridge.Tests.Performance.Internal
{
    public class ServiceScope
    {
        public required ICommandSender CommandSender { get; init; }
        public required IMediator Mediator { get; init; }
        public required CalculateSumService Service { get; init; }
    }
}