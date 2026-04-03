using CatInvaders.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace CatInvaders.API.Data;

/// <summary>
/// EF Core database context for the Cat Invaders application.
/// </summary>
public sealed class CatInvadersDbContext : DbContext
{
    /// <inheritdoc />
    public CatInvadersDbContext(DbContextOptions<CatInvadersDbContext> options)
        : base(options)
    {
    }

    /// <summary>Gets or sets the high scores table.</summary>
    public DbSet<HighScore> HighScores => Set<HighScore>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HighScore>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.PlayerName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Score)
                  .IsRequired();

            entity.Property(e => e.LevelReached)
                  .IsRequired(false);

            entity.Property(e => e.CreatedAtUtc)
                  .IsRequired();

            // Check constraint — Score must be non-negative
            entity.ToTable(t => t.HasCheckConstraint("CK_HighScores_Score", "\"Score\" >= 0"));

            // Composite index for the default sort order (Score DESC, CreatedAtUtc ASC)
            entity.HasIndex(e => new { e.Score, e.CreatedAtUtc })
                  .HasDatabaseName("IX_HighScores_Score_CreatedAtUtc");
        });
    }
}
