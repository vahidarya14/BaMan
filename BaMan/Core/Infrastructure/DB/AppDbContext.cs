using Microsoft.EntityFrameworkCore;
using BaManPubSub.Core.Domain;

namespace BaManPubSub.Core.Infrastructure.DB;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
    }
}
