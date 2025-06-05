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
    public DbSet<Estimate> Estimates { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<FinancialData>().HasOne(t => t.Company).WithMany(c => c.FinancialData).HasForeignKey(t => t.CompanyId);
        builder.Entity<Estimate>().HasIndex(e => new { e.CompanyId, e.AppUserId }).IsUnique(); 
        builder.Entity<Estimate>().HasOne(e => e.AppUser).WithMany(u => u.Estimates).HasForeignKey(e => e.AppUserId);
        builder.Entity<Estimate>().HasOne(e => e.Company).WithMany(u => u.Estimate).HasForeignKey(e => e.CompanyId);
    }
}