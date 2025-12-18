using CommandBridge.Interfaces;
using CommandBridge.Models;
using CommandBridge.Extensions;
using CommandBridge.Tests.Stubs;

namespace CommandBridge.Tests.Extensions
{
	public class TypeExtensions_IsInterceptorOfTests
	{
        [Theory]
        [InlineData(typeof(CommandOne), typeof(InterceptorOne), true)]
        [InlineData(typeof(CommandOne), typeof(InterceptorTwo), false)]
        [InlineData(typeof(CommandTwo), typeof(InterceptorTwo), true)]
        [InlineData(typeof(CommandThree<>), typeof(InterceptorThree<>), true)]
        [InlineData(typeof(CommandFour<>), typeof(InterceptorThree<>), false)]
        [InlineData(typeof(CommandThree<>), typeof(InterceptorFour<>), false)]
        [InlineData(typeof(CommandFour<>), typeof(InterceptorFour<>), true)]
        [InlineData(typeof(CommandOne), typeof(InterceptorAnyCommand<,>), true)]
        [InlineData(typeof(CommandTwo), typeof(InterceptorAnyCommand<,>), true)]
        [InlineData(typeof(CommandThree<>), typeof(InterceptorAnyCommand<,>), true)]
        [InlineData(typeof(CommandFour<>), typeof(InterceptorAnyCommand<,>), true)]
        public void Tests(Type command, Type interceptor, bool expectedResult)
        {
            var actualResult = interceptor.IsInterceptorOf(command);

            Assert.True(actualResult == expectedResult);
        }

        private record CommandOne(int Value) : ICommand<int>;
        private record CommandTwo(string Value) : ICommand<string>;
        private record CommandThree<T>(T Value) : ICommand<T>;
        private record CommandFour<T>(T Value) : ICommand<T>;

        private class InterceptorOne : CommandInterceptorStub<CommandOne, int> { }
        private class InterceptorTwo : CommandInterceptorStub<CommandTwo, string> { }
        private class InterceptorThree<T> : CommandInterceptorStub<CommandThree<T>, T> { }
        private class InterceptorFour<T> : CommandInterceptorStub<CommandFour<T>, T> { }

        private class InterceptorAnyCommand<TCommand, TResult> : CommandInterceptorStub<TCommand, TResult>
            where TCommand : ICommand<TResult>
        {
        }
    }
}