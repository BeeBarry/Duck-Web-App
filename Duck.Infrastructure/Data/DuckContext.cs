using Duck.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Duck.Infrastructure.Data;

public class DuckContext : DbContext
{
    public DuckContext(DbContextOptions<DuckContext> options) : base(options)
    {
    }

    public DbSet<Core.Models.Duck> Ducks => Set<Core.Models.Duck>();
    public DbSet<Quote> Quotes => Set<Quote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfigurerar relationen mellan Duck och Quotes
        modelBuilder.Entity<Core.Models.Duck>()
            .HasMany(d => d.Quotes)
            .WithOne(q => q.Duck)
            .HasForeignKey(q => q.DuckId);

        // Konfigurerar hur QuoteType ska sparas i databasen
        modelBuilder.Entity<Quote>()
            .Property(q => q.Type)
            .HasConversion<String>();
    }
}