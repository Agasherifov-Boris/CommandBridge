using CommandBridge.Tests.DemoProjects.Shop.Abstractions;
using CommandBridge.Tests.DemoProjects.Shop.Entities;
using CommandBridge.Tests.DemoProjects.Shop.Interfaces;
using CommandBridge.Tests.DemoProjects.Shop.Repositories;

namespace CommandBridge.Tests.DemoProjects.Shop.CommandHandlers;

public class CreateClientCommandHandler(ClientRepository repository) 
    : CreateEntityCommandHandlerBase<Client>(repository)
{
}