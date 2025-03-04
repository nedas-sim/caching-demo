using DataStoreDemo.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataStoreDemo.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<KeyValueMap> KeyValueMaps { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<KeyValueMap>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Value).IsRequired();
            entity.HasIndex(entity => entity.Id).IsUnique();
        });
    }
}
