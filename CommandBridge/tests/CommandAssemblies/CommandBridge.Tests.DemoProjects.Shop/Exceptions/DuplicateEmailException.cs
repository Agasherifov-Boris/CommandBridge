namespace CommandBridge.Tests.DemoProjects.Shop.Exceptions
{
    public class DuplicateEmailException() : Exception("User with the same email already exists")
    {
    }
}