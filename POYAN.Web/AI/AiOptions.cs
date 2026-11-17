namespace POYAN.Web.AI;

public sealed class AiOptions
{
    public const string SectionName = "Ai";

    // Full OpenAI-compatible chat completions endpoint.
    public string Endpoint { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 60;
}
