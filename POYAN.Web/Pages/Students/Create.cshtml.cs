using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POYAN.Web.Data;
using POYAN.Web.Models;

namespace POYAN.Web.Pages.Students;

public sealed class CreateModel(ApplicationDbContext db) : PageModel
{
    [BindProperty]
    public StudentInput Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue("school_id"), out var schoolId))
            return Forbid();

        if (!ModelState.IsValid)
            return Page();

        db.Students.Add(new Student
        {
            SchoolId = schoolId,
            StudentCode = Input.StudentCode.Trim(),
            FirstName = Input.FirstName.Trim(),
            LastName = Input.LastName.Trim(),
            Grade = Input.Grade?.Trim(),
            BirthDate = Input.BirthDate
        });

        await db.SaveChangesAsync(cancellationToken);
        return RedirectToPage("Index");
    }

    public sealed class StudentInput
    {
        [Required, StringLength(40)]
        [Display(Name = "کد دانش‌آموزی")]
        public string StudentCode { get; set; } = string.Empty;

        [Required, StringLength(100)]
        [Display(Name = "نام")]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(30)]
        [Display(Name = "پایه تحصیلی")]
        public string? Grade { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateOnly? BirthDate { get; set; }
    }
}
