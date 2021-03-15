using Microsoft.EntityFrameworkCore;
using PawsitivityScheduler.Data.Breeds;

namespace PawsitivityScheduler.Data
{
    public class SchedulingContext : DbContext
    {
        public DbSet<Breed> Breeds { get; set; }

        public DbSet<Dog> Dogs { get; set; }

        public SchedulingContext(DbContextOptions<SchedulingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Dog>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID).HasColumnType("CHAR(36)");
                entity.Property(e => e.Name).IsRequired();
                entity.HasOne(e => e.Breed).WithMany();
                entity.Property(e => e.BirthDate).HasColumnType("TIMESTAMP");
            });

            builder.Entity<Breed>(entity =>
            {
                entity.HasKey(e => e.Name);
                entity.Property(e => e.Group).IsRequired();
                entity.Property(e => e.Size).IsRequired();
                entity.Property(e => e.GroomMinutes).IsRequired();
                entity.Property(e => e.BathMinutes).IsRequired();
            });
        }
    }
}
