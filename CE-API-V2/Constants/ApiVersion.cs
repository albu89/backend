using System.Reflection;
namespace CE_API_V2.Constants;

public static class ApiVersion
{
    private static string? _commitId = null;
    public static string CommitId
    {
        get
        {
            return _commitId ?? (_commitId = Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(attr => attr.Key == "GitHash")?.Value);
        }
    }
}