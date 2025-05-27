using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<FinancialData> FinancialData { get; set; }
    public DbSet<Company> Companies { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<FinancialData>().HasOne(t => t.Company).WithMany(c => c.FinancialData).HasForeignKey(t => t.CompanyId);
        //builder.Entity<FinancialData>().ToTable("FinancialData");
        //builder.Entity<AppUser>().ToTable("AspNetUsers");
        //builder.Entity<IdentityRole>().ToTable("AspNetRoles");
        //builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
        //builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
        //builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
        //builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
        //builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
    }
}