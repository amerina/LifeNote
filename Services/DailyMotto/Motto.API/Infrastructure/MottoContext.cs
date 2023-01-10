using Microsoft.EntityFrameworkCore;
using Motto.API.Infrastructure.EntityConfigurations;
using Motto.API.Model;

namespace Motto.API.Infrastructure
{
    public class MottoContext : DbContext
    {
        public MottoContext(DbContextOptions<MottoContext> options) : base(options)
        {
        }
        public DbSet<MottoItem> MottoItems { get; set; }
        public DbSet<MottoLanguage> MottoLanguages { get; set; }
        public DbSet<MottoType> MottoTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MottoItemEntityTypeConfiguration());
            builder.ApplyConfiguration(new MottoLanguageEntityTypeConfiguration());
            builder.ApplyConfiguration(new MottoTypeEntityTypeConfiguration());
        }
    }
}
