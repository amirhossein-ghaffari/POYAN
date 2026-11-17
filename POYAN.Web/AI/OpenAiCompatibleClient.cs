using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace POYAN.Web.AI;

public sealed class OpenAiCompatibleClient(
    HttpClient httpClient,
    IOptions<AiOptions> options) : IAiService
{
    private readonly AiOptions _options = options.Value;

    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("Prompt cannot be empty.", nameof(prompt));

        if (!Uri.TryCreate(_options.Endpoint, UriKind.Absolute, out var endpoint))
            throw new InvalidOperationException("AI endpoint is not configured with a valid absolute URL.");

        if (string.IsNullOrWhiteSpace(_options.ApiKey) || string.IsNullOrWhiteSpace(_options.Model))
            throw new InvalidOperationException("AI API key and model must be configured before use.");

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        request.Content = JsonContent.Create(new
        {
            model = _options.Model,
            messages = new[] { new { role = "user", content = prompt } }
        });

        using var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

        if (document.RootElement.TryGetProperty("choices", out var choices)
            && choices.ValueKind == JsonValueKind.Array
            && choices.GetArrayLength() > 0
            && choices[0].TryGetProperty("message", out var message)
            && message.TryGetProperty("content", out var content)
            && content.ValueKind == JsonValueKind.String)
        {
            return content.GetString() ?? string.Empty;
        }

        throw new InvalidOperationException("The configured AI provider returned an unsupported response format.");
    }
}
