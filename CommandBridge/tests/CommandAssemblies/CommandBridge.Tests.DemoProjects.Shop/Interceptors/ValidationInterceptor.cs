using CommandBridge.Interfaces;
using CommandBridge.Models;
using CommandBridge.Tests.DemoProjects.Shop.Abstractions;
using CommandBridge.Tests.DemoProjects.Shop.Commands;
using System.ComponentModel.DataAnnotations;

namespace CommandBridge.Tests.DemoProjects.Shop.Interceptors;

public class ValidationInterceptor<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
    {
        var validationContext = new ValidationContext(context.Command);
        Validator.ValidateObject(context.Command, validationContext, validateAllProperties: true);

        return next(context);
    }
}