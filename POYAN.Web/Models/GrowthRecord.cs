namespace POYAN.Web.Models;

public sealed class GrowthRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SchoolId { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public string Category { get; set; } = "Educational";
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Score { get; set; }
    public DateTime RecordedAtUtc { get; set; } = DateTime.UtcNow;
    public string? RecordedByUserId { get; set; }
}