namespace example_mcp_server.Authentication;

/// <summary>
/// Hardcoded API key -> username map. Each user of this server gets their own key
/// here; requests are attributed to whichever user's key was presented.
/// </summary>
public static class ApiKeyStore
{
    public static readonly IReadOnlyDictionary<string, string> ApiKeysToUsers =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["replace-with-alices-key"] = "alice",
            ["replace-with-bobs-key"] = "bob",
        };
}
