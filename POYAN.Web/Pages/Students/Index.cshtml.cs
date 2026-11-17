using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POYAN.Web.Data;
using POYAN.Web.Models;

namespace POYAN.Web.Pages.Students;

public sealed class IndexModel(ApplicationDbContext db) : PageModel
{
    public IReadOnlyList<Student> Students { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Students = await db.Students
            .AsNoTracking()
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName)
            .ToListAsync(cancellationToken);
    }
}
