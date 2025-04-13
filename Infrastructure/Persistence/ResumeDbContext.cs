using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Infrastructure.Persistence
{
    public class ResumeDbContext : DbContext
    {
        public ResumeDbContext(DbContextOptions<ResumeDbContext> options) : base(options) { }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ExperienceItem> ExperienceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var stringListConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
            );

            modelBuilder.Entity<Resume>()
                .Property(r => r.Skills)
                .HasConversion(stringListConverter);

            modelBuilder.Entity<ExperienceItem>()
                .Property(e => e.Responsibilities)
                .HasConversion(stringListConverter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
