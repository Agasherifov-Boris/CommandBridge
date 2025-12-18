using System;
using CommandBridge.Models;

namespace CommandBridge.Interfaces
{
    public interface ICommandDelegateFactory
    {
        CommandDelegate<TResult> Create<TResult>(Type commandType);
    }
}