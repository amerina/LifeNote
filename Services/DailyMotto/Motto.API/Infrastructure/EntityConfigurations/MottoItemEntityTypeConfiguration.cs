using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motto.API.Model;

namespace Motto.API.Infrastructure.EntityConfigurations
{
    public class MottoItemEntityTypeConfiguration : IEntityTypeConfiguration<MottoItem>
    {
        public void Configure(EntityTypeBuilder<MottoItem> builder)
        {
            builder.ToTable("Motto");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("motto_hilo")
                .IsRequired();

            builder.Property(cb => cb.Author)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.Property(cb => cb.Content)
                 .IsRequired(true)
                 .HasMaxLength(300);

            builder.HasOne(ci => ci.MottoLanguage)
                  .WithMany()
                  .HasForeignKey(ci => ci.MottoLanguageId);

            builder.HasOne(ci => ci.MottoType)
                 .WithMany()
                 .HasForeignKey(ci => ci.MottoTypeId);
        }

    }
}
