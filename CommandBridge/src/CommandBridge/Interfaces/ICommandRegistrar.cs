using CommandBridge.Configuration;

namespace CommandBridge.Interfaces
{
    public interface ICommandRegistrar
    {
        void Register(CommandBridgeConfigurationBuilder builder);
    }
}