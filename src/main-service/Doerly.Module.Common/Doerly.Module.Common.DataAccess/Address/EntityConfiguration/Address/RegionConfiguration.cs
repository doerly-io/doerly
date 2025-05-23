using Doerly.Module.Common.DataAccess.Address.Constants;
using Doerly.Module.Common.DataAccess.Address.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Common.DataAccess.Address.EntityConfiguration.Address;

public class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable(AddressDbConstants.Tables.Region, AddressDbConstants.AddressSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    }
}
