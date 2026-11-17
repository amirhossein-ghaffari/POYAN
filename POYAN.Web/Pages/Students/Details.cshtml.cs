using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POYAN.Web.Data;
using POYAN.Web.Models;

namespace POYAN.Web.Pages.Students;

public sealed class DetailsModel(ApplicationDbContext db) : PageModel
{
    public Student? Student { get; private set; }

    [BindProperty]
    public GrowthInput Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        Student = await LoadStudentAsync(id, cancellationToken);
        return Student is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostAddRecordAsync(Guid id, CancellationToken cancellationToken)
    {
        Student = await db.Students.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (Student is null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            Student = await LoadStudentAsync(id, cancellationToken);
            return Page();
        }

        var allowedCategories = new[] { "Educational", "Sporting", "Behavioral", "Physical" };
        if (!allowedCategories.Contains(Input.Category, StringComparer.Ordinal))
        {
            ModelState.AddModelError("Input.Category", "دسته‌بندی انتخاب‌شده معتبر نیست.");
            Student = await LoadStudentAsync(id, cancellationToken);
            return Page();
        }

        db.GrowthRecords.Add(new GrowthRecord
        {
            SchoolId = Student.SchoolId,
            StudentId = Student.Id,
            Category = Input.Category,
            Title = Input.Title.Trim(),
            Description = Input.Description?.Trim(),
            Score = Input.Score,
            RecordedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        });

        await db.SaveChangesAsync(cancellationToken);
        return RedirectToPage(new { id });
    }

    private Task<Student?> LoadStudentAsync(Guid id, CancellationToken cancellationToken) =>
        db.Students.AsNoTracking()
            .Include(x => x.GrowthRecords.OrderByDescending(r => r.RecordedAtUtc))
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public sealed class GrowthInput
    {
        [Required]
        [Display(Name = "حوزه ارزیابی")]
        public string Category { get; set; } = "Educational";

        [Required, StringLength(200)]
        [Display(Name = "عنوان")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        [Display(Name = "شرح و شواهد")]
        public string? Description { get; set; }

        [Range(typeof(decimal), "0", "100")]
        [Display(Name = "امتیاز (اختیاری، از ۰ تا ۱۰۰)")]
        public decimal? Score { get; set; }
    }
}
