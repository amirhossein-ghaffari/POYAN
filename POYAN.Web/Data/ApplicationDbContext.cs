using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POYAN.Web.Models;
using System.Security.Claims;

namespace POYAN.Web.Data;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<ApplicationUser>(options)
{
    private Guid? CurrentSchoolId =>
        Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirstValue("school_id"), out var id)
            ? id
            : null;

    private bool IsSuperAdmin =>
        httpContextAccessor.HttpContext?.User.IsInRole("SuperAdmin") == true;

    public DbSet<School> Schools => Set<School>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<GrowthRecord> GrowthRecords => Set<GrowthRecord>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<School>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.City).HasMaxLength(100);
            entity.HasQueryFilter(x => IsSuperAdmin || (CurrentSchoolId != null && x.Id == CurrentSchoolId));
        });

        builder.Entity<Student>(entity =>
        {
            entity.HasIndex(x => new { x.SchoolId, x.StudentCode }).IsUnique();
            entity.Property(x => x.StudentCode).HasMaxLength(40).IsRequired();
            entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            entity.HasOne(x => x.School).WithMany(x => x.Students)
                .HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(x => IsSuperAdmin || (CurrentSchoolId != null && x.SchoolId == CurrentSchoolId));
        });

        builder.Entity<GrowthRecord>(entity =>
        {
            entity.Property(x => x.Category).HasMaxLength(40).IsRequired();
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.HasOne(x => x.Student).WithMany(x => x.GrowthRecords)
                .HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Cascade);
            entity.HasQueryFilter(x => IsSuperAdmin || (CurrentSchoolId != null && x.SchoolId == CurrentSchoolId));
        });
    }
}