# CommandBridge

A lightweight and extensible implementation of the **Mediator pattern** for .NET, designed for clean application architecture, CQRS-style workflows, and high performance.

---

## Features

- Strongly typed handlers
- Asynchronous execution
- Pipeline with interceptors
- Minimal allocations
- Easy integration with Dependency Injection
- .NET Standard 2.1

---

## Installation

```bash
dotnet add package SoftBag.CommandBridge
```


## Quick Start
Define a command

```cs
public record CreateUserCommand(string Name, string Email) : ICommand<Guid>;
```

Implement a handler

```cs
public class CreateUserHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public ValueTask<Guid> HandleAsync(CreateUserCommand command, CancellationToken ct)
    {
        Console.WriteLine("CreateUserHandler: user created");
        return ValueTask.FromResult(Guid.NewGuid());
    }
}
```

Register CommandBridge in the DI

```cs
builder.Services.AddCommandBridge(cfg => 
{
    cfg.AddCommand<CreateUserHandler, CreateUserCommand, Guid>();
});
```

Inject ICommandSender and send command

```cs
[Route("api/users")]
public class UsersController(ICommandSender commandSender) : ApiControllerBase
{
    [HttpPost]
    public async Task<Guid> Post([FromBody] CreateUserCommand command)
    {
        return await commandSender.SendAsync(command);
    }
}
```

## Interceptors

Interceptors allow you to wrap handler execution with cross-cutting concerns such as logging, validation, caching, or transactions.

Execution order:
```
GlobalInterceptor 1
  GlobalInterceptor 2
    CommandInterceptor 1
      CommandInterceptor 2
        Handler
      CommandInterceptor 2
    CommandInterceptor 1
  GlobalInterceptor 2
GlobalInterceptor 1
```

Example:
```cs
public class GlobalInterceptor1<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
    {
        Console.WriteLine("GlobalInterceptor1: Starting command execution");

        var result = await next(context);

        logger.Log("GlobalInterceptor1: Command execution completed");

        return result;
    }
}

public class GlobalInterceptor2<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
    {
        Console.WriteLine("GlobalInterceptor2: Starting command execution");

        var result = await next(context);

        logger.Log("GlobalInterceptor2: Command execution completed");

        return result;
    }
}

public class CommandInterceptor1 : ICommandInterceptor<CreateUserCommand, Guid>
{
    public ValueTask<User> HandleAsync(ICommandContext<CreateUserCommand, Guid> context, CommandDelegate<CreateUserCommand, Guid> next)
    {
        Console.WriteLine("CommandInterceptor1: Starting command execution");

        var result = await next(context);

        logger.Log("CommandInterceptor1: Command execution completed");

        return result;
    }
}
public class CommandInterceptor2 : ICommandInterceptor<CreateUserCommand, Guid>
{
    public ValueTask<User> HandleAsync(ICommandContext<CreateUserCommand, Guid> context, CommandDelegate<CreateUserCommand, Guid> next)
    {
        Console.WriteLine("CommandInterceptor2: Starting command execution");

        var result = await next(context);

        logger.Log("CommandInterceptor2: Command execution completed");

        return result;
    }
}
```

Add to DI

```cs
builder.Services.AddCommandBridge(cfg => 
{
    cfg.AddInterceptor(typeof(GlobalInterceptor1<,>));
    cfg.AddInterceptor(typeof(GlobalInterceptor2<,>));

    cfg.AddCommand<CreateUserHandler, CreateUserCommand, Guid>()
        .WithInterceptor<CommandInterceptor1>()
        .WithInterceptor<CommandInterceptor2>();
});
```

Result:
```
GlobalInterceptor1: Starting command execution
GlobalInterceptor2: Starting command execution
CommandInterceptor1: Starting command execution
CommandInterceptor2: Starting command execution
CreateUserHandler: user created
CommandInterceptor2: Command execution completed
CommandInterceptor1: Command execution completed
GlobalInterceptor2: Command execution completed
GlobalInterceptor1: Command execution completed
```

# Lambda interceptors


```cs
builder.Services.AddCommandBridge(cfg => 
{
    cfg.AddInterceptor(typeof(GlobalInterceptor1<,>));
    cfg.AddInterceptor(typeof(GlobalInterceptor2<,>));

    cfg.AddCommand<CreateUserHandler, CreateUserCommand, Guid>()
        .WithInterceptor<CommandInterceptor1>()
        .WithInterceptor(async (ctx, next) =>
        {
            Console.WriteLine("CommandInterceptor2: Starting command execution");

            var result = await next(context);

            logger.Log("CommandInterceptor2: Command execution completed");

            return result;
        });
});
```

## Registrars

# Scan assemblies

```cs
builder.Services.AddCommandBridge(cfg => 
{
    cfg.AddInterceptor(typeof(GlobalInterceptor1<,>));
    cfg.AddInterceptor(typeof(GlobalInterceptor2<,>));

    cfg.FromAssemblyOf<Program>();
});
```

# Scan with options

Filter - Filter to exclude command-handler pairs. Return true to include the pair, false to exclude it.
Interceptors - Find interceptors for each command in the assembly, by default false
```cs
builder.Services.AddCommandBridge(cfg => 
{
    cfg.AddInterceptor(typeof(GlobalInterceptor1<,>));
    cfg.AddInterceptor(typeof(GlobalInterceptor2<,>));

    var scanOptions = new ScanAssemblyOptions()
    {
        Filter = (command, handler) => true,
        Interceptors = false
    };
    cfg.FromAssemblyOf<Program>(scanOptions);
});
```

# Custom registar

```cs
public class MyCommandRegistrar : ICommandRegistrar
{
    public void Register(CommandBridgeConfigurationBuilder builder)
    {
        builder.AddInterceptor(typeof(GlobalInterceptor1<,>));
        builder.AddInterceptor(typeof(GlobalInterceptor2<,>));
        builder.AddCommand<CreateUserHandler, CreateUserCommand, Guid>()
            .WithInterceptor<CommandInterceptor1>()
            .WithInterceptor<CommandInterceptor2>();
    }
}
```
DI
```cs
builder.Services.AddCommandBridge(cfg => 
{
    cfg.From(new MyCommandRegistrar());
});
```

# Attribute interceptor

You can add interceptors via attribute to Command or Handler

```cs
[UseInterceptor(typeof(CommandInterceptor1))]
[UseInterceptor(typeof(CommandInterceptor2))]
public class CreateUserHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public ValueTask<Guid> HandleAsync(CreateUserCommand command, CancellationToken ct)
    {
        Console.WriteLine("CreateUserHandler: user created");
        return ValueTask.FromResult(Guid.NewGuid());
    }
}
```

## Lifetime

By default lifetime of Handlers and Interceptors are Transient, but you can change it

By attribute:
```cs
[Lifetime(ServiceLifetime.Singleton)]
public class CreateUserHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public ValueTask<Guid> HandleAsync(CreateUserCommand command, CancellationToken ct)
    {
        Console.WriteLine("CreateUserHandler: user created");
        return ValueTask.FromResult(Guid.NewGuid());
    }
}
```

In DI:
```cs
builder.Services.AddCommandBridge(cfg =>
{
    cfg.AddInterceptor(typeof(GlobalInterceptor1<,>), lifetime: ServiceLifetime.Singleton);
    cfg.AddInterceptor(typeof(GlobalInterceptor2<,>), lifetime: ServiceLifetime.Singleton);

    cfg.AddCommand<CreateUserHandler, CreateUserCommand, Guid>(lifetime: ServiceLifetime.Singleton)
        .WithInterceptor<CommandInterceptor1>(lifetime: ServiceLifetime.Singleton)
        .WithInterceptor<CommandInterceptor2>(lifetime: ServiceLifetime.Singleton);
});
```

## Performance

Benchmark methods:
```cs
[Benchmark(Baseline = true)]
public async Task<int> Service()
{
    var command = new CalculateSumCommand(16, 4);
    var service = ServiceProvider.GetRequiredService<CalculateSumService>();
    return await service.CalculateAsync(command);
}

[Benchmark]
public async Task<int> CommandBridge()
{
    var command = new CalculateSumCommand(16, 4);
    var commandSender = ServiceProvider.GetRequiredService<ICommandSender>();
    return await commandSender.SendAsync(command);
}

[Benchmark]
public async Task<int> MediatR()
{
    var request = new CalculateSumRequest(16, 4);
    var mediator = ServiceProvider.GetRequiredService<IMediator>();
    return await mediator.Send(request);
}
```



# No interceptors
Setup:
```cs
ServiceProvider = CreateServiceProvider(services =>
{
    services.AddCommandBridge(opts =>
    {
        opts.AddCommand<CalculateSumCommand, CalculateSumCommandHandler, int>();
    });

    services.AddMediatR(opts =>
    {
        opts.RegisterServicesFromAssemblyContaining<CalculateSumCommand>();
    });

    services.AddTransient<CalculateSumService>();
});
```
Results
```
| Method        | Mean     | Error   | StdDev  | Ratio | RatioSD | Allocated | Alloc Ratio |
|-------------- |---------:|--------:|--------:|------:|--------:|----------:|------------:|
| Service       | 117.0 ns | 2.28 ns | 3.55 ns |  1.00 |    0.04 |     240 B |        1.00 |
| CommandBridge | 159.3 ns | 1.85 ns | 1.64 ns |  1.36 |    0.04 |     304 B |        1.27 |
| MediatR       | 202.8 ns | 3.78 ns | 6.11 ns |  1.73 |    0.07 |     440 B |        1.83 |
```

# 1 interceptor
Setup:
```cs
ServiceProvider = CreateServiceProvider(services =>
{
    services.AddCommandBridge(opts =>
    {
        opts.AddInterceptor(typeof(AsyncInterceptor1<,>));
        opts.AddCommand<CalculateSumCommand, CalculateSumCommandHandler, int>();
    });

    services.AddMediatR(opts =>
    {
        opts.RegisterServicesFromAssemblyContaining<CalculateSumCommand>();
    });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior1<,>));
    services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor1>();

    services.AddTransient<CalculateSumService>();
}); 
```
Results
```
| Method        | Mean     | Error   | StdDev  | Ratio | RatioSD | Allocated | Alloc Ratio |
|-------------- |---------:|--------:|--------:|------:|--------:|----------:|------------:|
| Service       | 189.8 ns | 3.83 ns | 8.87 ns |  1.00 |    0.06 |     360 B |        1.00 |
| CommandBridge | 229.7 ns | 4.61 ns | 9.31 ns |  1.21 |    0.07 |     368 B |        1.02 |
| MediatR       | 275.8 ns | 5.45 ns | 5.35 ns |  1.46 |    0.07 |     704 B |        1.96 |
```

# 3 interceptors
Setup:
```cs
ServiceProvider = CreateServiceProvider(services =>
{
    services.AddCommandBridge(opts =>
    {
        opts.AddInterceptor(typeof(AsyncInterceptor1<,>));
        opts.AddInterceptor(typeof(AsyncInterceptor2<,>));
        opts.AddInterceptor(typeof(AsyncInterceptor3<,>));
        opts.AddCommand<CalculateSumCommand, CalculateSumCommandHandler, int>();
    });


    services.AddMediatR(opts =>
    {
        opts.RegisterServicesFromAssemblyContaining<CalculateSumCommand>();
    });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior1<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior2<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior3<,>));


    services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor1>();
    services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor2>();
    services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor3>();

    services.AddTransient<CalculateSumService>();
}); 
```
Results
```
| Method        | Mean     | Error   | StdDev   | Median   | Ratio | RatioSD | Allocated | Alloc Ratio |
|-------------- |---------:|--------:|---------:|---------:|------:|--------:|----------:|------------:|
| Service       | 286.2 ns | 4.40 ns |  3.67 ns | 284.8 ns |  1.00 |    0.02 |     440 B |        1.00 |
| CommandBridge | 369.5 ns | 6.93 ns |  6.48 ns | 367.3 ns |  1.29 |    0.03 |     704 B |        1.60 |
| MediatR       | 410.4 ns | 7.81 ns | 17.63 ns | 404.3 ns |  1.43 |    0.06 |    1136 B |        2.58 |
```

# 5 interceptors
Setup:
```cs
ServiceProvider = CreateServiceProvider(services =>
{
    services.AddCommandBridge(opts =>
    {
        opts.AddInterceptor(typeof(AsyncInterceptor1<,>));
        opts.AddInterceptor(typeof(AsyncInterceptor2<,>));
        opts.AddInterceptor(typeof(AsyncInterceptor3<,>));
        opts.AddInterceptor(typeof(AsyncInterceptor4<,>));
        opts.AddInterceptor(typeof(AsyncInterceptor5<,>));
        opts.AddCommand<CalculateSumCommand, CalculateSumCommandHandler, int>();
    });


    services.AddMediatR(opts =>
    {
        opts.RegisterServicesFromAssemblyContaining<CalculateSumCommand>();
    });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior1<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior2<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior3<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior4<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AsyncPipelineBehavior5<,>));


    services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor1>();
    services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor2>();
    services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor3>();
    services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor4>();
    services.AddTransient<ICalculateSumInterceptor, AsyncCalculateSumInterceptor5>();

    services.AddTransient<CalculateSumService>();
}); 
```
Results
```
| Method        | Mean     | Error   | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|-------------- |---------:|--------:|---------:|------:|--------:|----------:|------------:|
| Service       | 389.0 ns | 4.69 ns |  4.16 ns |  1.00 |    0.01 |     520 B |        1.00 |
| CommandBridge | 512.3 ns | 9.85 ns | 13.15 ns |  1.32 |    0.04 |     944 B |        1.82 |
| MediatR       | 550.5 ns | 8.69 ns |  7.25 ns |  1.42 |    0.02 |    1568 B |        3.02 |
```

## Why CommandBridge?

CommandBridge is designed as a high-performance alternative to traditional Mediator implementations,
focusing on:

- Minimal runtime overhead
- Explicit command registration
- Predictable interceptor execution order
- Zero reflection during command execution