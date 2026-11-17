using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POYAN.Web.Data;

namespace POYAN.Web.Pages.Dashboard;

public sealed class IndexModel(ApplicationDbContext db) : PageModel
{
    public int StudentsCount { get; private set; }
    public int GrowthRecordsCount { get; private set; }
    public int ActiveSchoolsCount { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        StudentsCount = await db.Students.CountAsync(cancellationToken);
        GrowthRecordsCount = await db.GrowthRecords.CountAsync(cancellationToken);
        ActiveSchoolsCount = await db.Schools.CountAsync(x => x.IsActive, cancellationToken);
    }
}
