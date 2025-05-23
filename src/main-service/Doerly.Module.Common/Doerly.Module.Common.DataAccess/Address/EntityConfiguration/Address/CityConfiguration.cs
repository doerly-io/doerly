using Doerly.Module.Common.DataAccess.Address.Constants;
using Doerly.Module.Common.DataAccess.Address.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Common.DataAccess.Address.EntityConfiguration.Address;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable(AddressDbConstants.Tables.City, AddressDbConstants.AddressSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.RegionId).IsRequired();
        
        builder.HasOne(x => x.Region)
            .WithMany()
            .HasForeignKey(x => x.RegionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(x => x.RegionId);
    }
}
