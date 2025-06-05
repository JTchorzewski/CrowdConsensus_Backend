using Microsoft.AspNetCore.Identity;

namespace Domain.Model;

public class AppUser : IdentityUser
{
    public ICollection<Estimate> Estimates { get; set; }
}