namespace CommandBridge.Tests.DemoProjects.Shop.Exceptions
{
    public class ForbiddenEmailBoxException(string emailBox) : Exception($"Email '{emailBox}' forbidden")
    {
    }
}