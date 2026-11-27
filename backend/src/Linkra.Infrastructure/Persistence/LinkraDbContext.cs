using Microsoft.EntityFrameworkCore;

namespace Linkra.Infrastructure.Persistence;

public class LinkraDbContext : DbContext
{
    public LinkraDbContext(DbContextOptions<LinkraDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LinkraDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}