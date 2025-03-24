using CC_Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CC_Infrastructure;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyToGroupConnection> CompaniesToGroupsConnection { get; set; }
    public DbSet<Group> Groups { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);                        

        builder.Entity<CompanyToGroupConnection>().HasKey(it => new {it.CompanyId, it.GroupId});
        builder.Entity<CompanyToGroupConnection>().HasOne<Company>(it => it.Company).WithMany(it => it.CompanyToGroupConnection).HasForeignKey(it => it.CompanyId);
        builder.Entity<CompanyToGroupConnection>().HasOne<Group>(it => it.Group).WithMany(it => it.CompanyToGroupConnection).HasForeignKey(it => it.GroupId);
    }
}