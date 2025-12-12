using Devices.Domain.Devices.Entities;
using Devices.Domain.Devices.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Devices.Infrastructure.Devices.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.Name)
               .HasMaxLength(DeviceSpecification.NameColumnSize)
               .IsRequired();

        builder.Property(x => x.Brand)
               .HasMaxLength(DeviceSpecification.BrandColumnSize)
               .IsRequired();

        builder.Property(x => x.State)
               .IsRequired();

        builder.Property(m => m.CreationTime)
               .HasColumnType("timestamp without time zone")
               .IsRequired();
    }
}
