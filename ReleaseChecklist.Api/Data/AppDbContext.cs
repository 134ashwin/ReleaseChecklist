using Microsoft.EntityFrameworkCore;
using ReleaseChecklist.Api.Models;

namespace ReleaseChecklist.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Release> Releases => Set<Release>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Release>(entity =>
            {
                entity.ToTable("releases");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired();

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("release_date");

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info");

                entity.Property(e => e.CompletedStepIds)
                    .HasColumnName("completed_step_ids")
                    .HasColumnType("jsonb");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at");

                // Ignore Status from DB mapping as it is calculated in C#
                entity.Ignore(e => e.Status);
            });
        }
    }
}
