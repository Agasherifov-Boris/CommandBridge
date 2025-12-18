namespace CommandBridge.Interfaces
{
    public interface ICommand
    {
    }

    public interface ICommand<T> : ICommand 
    { 
    }
}