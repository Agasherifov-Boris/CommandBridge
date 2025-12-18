namespace CommandBridge.Tests.DemoProjects.Shop.Abstractions
{
    public abstract record Entity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}