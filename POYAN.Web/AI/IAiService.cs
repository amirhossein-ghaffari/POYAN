namespace POYAN.Web.AI;

public interface IAiService
{
    Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default);
}
