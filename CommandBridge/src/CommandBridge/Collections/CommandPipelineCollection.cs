using CommandBridge.Interfaces;
using CommandBridge.Models;
using System;
using System.Collections.Concurrent;

namespace CommandBridge.Collections
{
    public class CommandPipelineCollection
    {
        protected readonly ConcurrentDictionary<Type, Delegate> Pipelines;

        #region Ctor

        public CommandPipelineCollection()
        {
            Pipelines = new ConcurrentDictionary<Type, Delegate>();
        }

        #endregion

        #region GetOrAdd

        public virtual CommandDelegate<TResult> GetOrAdd<TResult>(Type commandType, Func<CommandDelegate<TResult>> factory)
        {
            return GetOrAddCore(commandType, factory);
        }

        protected virtual T GetOrAddCore<T>(Type commandType, Func<T> factory)
            where T : Delegate
        {
            var del = Pipelines.GetOrAdd(commandType, _ => factory());

            if (del is T pipeline)
            {
                return pipeline;
            }
            else
            {
                throw new InvalidOperationException($"The command pipeline for {commandType.Name} is not of the expected type.");
            }
        }

        #endregion

    }
}