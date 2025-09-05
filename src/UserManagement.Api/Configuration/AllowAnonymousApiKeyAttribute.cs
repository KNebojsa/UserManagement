namespace UserManagement.Api.Configuration
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllowAnonymousApiKeyAttribute : Attribute
    {
    }
}