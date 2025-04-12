using Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<Spolki> Spolki { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Quotation> Quotations { get; set; }
    public DbSet<CompanyToGroupConnection> CompaniesToGroupsConnection { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<FinancialData> FinancialData { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);                        

        builder.Entity<CompanyToGroupConnection>().HasKey(it => new {it.SpolkiId, it.GroupId});
        builder.Entity<CompanyToGroupConnection>().HasOne<Spolki>(it => it.Spolki).WithMany(it => it.CompanyToGroupConnection).HasForeignKey(it => it.SpolkiId);
        builder.Entity<CompanyToGroupConnection>().HasOne<Group>(it => it.Group).WithMany(it => it.CompanyToGroupConnection).HasForeignKey(it => it.GroupId);
        builder.Entity<Quotation>().HasOne(q => q.Company).WithMany(c => c.Quotations).HasForeignKey(q => q.CompanyId);
    }
}