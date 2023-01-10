using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motto.API.Model;

namespace Motto.API.Infrastructure.EntityConfigurations
{
    public class MottoLanguageEntityTypeConfiguration : IEntityTypeConfiguration<MottoLanguage>
    {
        public void Configure(EntityTypeBuilder<MottoLanguage> builder)
        {
            builder.ToTable("MottoLanguage");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("motto_language_hilo")
                .IsRequired();

            builder.Property(cb => cb.Language)
                .IsRequired()
                .HasMaxLength(100);
        }

    }
}
