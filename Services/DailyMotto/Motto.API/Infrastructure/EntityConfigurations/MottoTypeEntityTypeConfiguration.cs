using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motto.API.Model;

namespace Motto.API.Infrastructure.EntityConfigurations
{
    public class MottoTypeEntityTypeConfiguration : IEntityTypeConfiguration<MottoType>
    {
        public void Configure(EntityTypeBuilder<MottoType> builder)
        {
            builder.ToTable("MottoType");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .UseHiLo("motto_type_hilo")
                .IsRequired();

            builder.Property(cb => cb.Type)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
