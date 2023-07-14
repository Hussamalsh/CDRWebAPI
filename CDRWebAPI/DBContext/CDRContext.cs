using CDRWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CDRWebAPI.DBContext;

public class CDRContext : DbContext, ICDRContext
{
    public DbSet<CDR> CDRs { get; set; }

    public CDRContext(DbContextOptions<CDRContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CDR>()
            .Property(c => c.Cost)
            .HasPrecision(18, 3);
    }
}
