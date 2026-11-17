using Microsoft.AspNetCore.Identity;

namespace POYAN.Web.Models;

public sealed class ApplicationUser : IdentityUser
{
    public Guid? SchoolId { get; set; }
}