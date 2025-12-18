using CommandBridge.Configuration;
using CommandBridge.Interfaces;
using CommandBridge.Models;
using CommandBridge.Registrars;
using CommandBridge.Tests.Stubs;

namespace CommandBridge.Tests.Configurators
{
    public class FromAssemblyCommandRegistrarTests
    {
        [Theory]
        [InlineData(typeof(Command1), typeof(Command1Interceptor1), typeof(Command1Interceptor2))]
        [InlineData(typeof(Command2), typeof(Command2Interceptor1), typeof(Command2Interceptor2))]
        [InlineData(typeof(Command3))]
        [InlineData(typeof(CommandGeneric1<>), typeof(CommandGeneric1Interceptor1<>), typeof(CommandGeneric1Interceptor2<>))]
        [InlineData(typeof(CommandGeneric2<>), typeof(CommandGeneric2Interceptor1<>), typeof(CommandGeneric2Interceptor2<>), typeof(CommandGeneric2Interceptor3<>))]
        public void GetCommandInterceptor(Type commandType, params Type[] expectedResult)
        {
            expectedResult = expectedResult ?? [];

            var configurator = new FromAssemblyCommandRegistrarWrapper(GetAllInterceptors());
            var interceptors = configurator.GetCommandInterceptors(commandType).ToList();

            var missed = expectedResult.Except(interceptors).ToList();

            if (missed.Count > 0)
                Assert.Fail($"""There are missed interceptors: [ {string.Join(", ", missed.Select(x => x.Name))}" ]""");

            var extra = interceptors.Except(expectedResult).ToList();
            if (extra.Count > 0)
                Assert.Fail($"""There are extra interceptors: [ {string.Join(", ", extra.Select(x => x.Name))}" ]""");
        }

        private IEnumerable<Type> GetAllInterceptors()
        {
            return new Type[]
            {
                typeof(GlobalInterceptor1<,>),
                typeof(GlobalInterceptor2<,>),
                typeof(GlobalInterceptor3<,>),
                typeof(Command1Interceptor1),
                typeof(Command1Interceptor2),
                typeof(Command2Interceptor1),
                typeof(Command2Interceptor2),
                typeof(CommandGeneric1Interceptor1<>),
                typeof(CommandGeneric1Interceptor2<>),
                typeof(CommandGeneric2Interceptor1<>),
                typeof(CommandGeneric2Interceptor2<>),
                typeof(CommandGeneric2Interceptor3<>)
            };
        }


        public class FromAssemblyCommandRegistrarWrapper(IEnumerable<Type> interceptors) 
            : FromAssemblyCommandRegistrar(typeof(FromAssemblyCommandRegistrarWrapper).Assembly, ScanAssemblyOptions.Default)
        {
            protected override IEnumerable<Type> GetAllInterceptors()
            {
                return interceptors;
            }

            public new IEnumerable<Type> GetCommandInterceptors(Type command)
            {
                return base.GetCommandInterceptors(command);
            }

        }

        public class GlobalInterceptor1<TCommand, TResult> : CommandInterceptorStub<TCommand, TResult>
            where TCommand : ICommand<TResult> { }


        public class GlobalInterceptor2<TCommand, TResult> : CommandInterceptorStub<TCommand, TResult>
            where TCommand : ICommand<TResult> { }

        public class GlobalInterceptor3<TCommand, TResult> : CommandInterceptorStub<TCommand, TResult>
            where TCommand : ICommand<TResult> { }

        public class Command1Interceptor1 : CommandInterceptorStub<Command1, int> { }
        public class Command1Interceptor2 : CommandInterceptorStub<Command1, int> { }
        public class Command2Interceptor1 : CommandInterceptorStub<Command2, string> { }
        public class Command2Interceptor2 : CommandInterceptorStub<Command2, string> { }
        public class CommandGeneric1Interceptor1<T> : CommandInterceptorStub<CommandGeneric1<T>, T> { }
        public class CommandGeneric1Interceptor2<T> : CommandInterceptorStub<CommandGeneric1<T>, T> { }
        public class CommandGeneric2Interceptor1<T> : CommandInterceptorStub<CommandGeneric2<T>, T> { }
        public class CommandGeneric2Interceptor2<T> : CommandInterceptorStub<CommandGeneric2<T>, T> { }
        public class CommandGeneric2Interceptor3<T> : ICommandInterceptor<CommandGeneric2<T>, T>
        {
            public ValueTask<T> HandleAsync(ICommandContext<CommandGeneric2<T>, T> context, CommandDelegate<CommandGeneric2<T>, T> next)
            {
                return next(context);
            }
        }

        public class Command1 : ICommand<int> { }
        public class Command2 : ICommand<string> { }
        public class Command3 : ICommand<string> { }
        public class CommandGeneric1<T> : ICommand<T> { }
        public class CommandGeneric2<T> : ICommand<T> { }
    }
}