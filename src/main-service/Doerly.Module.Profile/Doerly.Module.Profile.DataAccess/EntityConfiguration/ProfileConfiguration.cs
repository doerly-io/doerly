using Doerly.Module.Profile.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Profile.DataAccess.EntityConfiguration;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.DateOfBirth);
        builder.Property(x => x.Sex);
        builder.Property(x => x.Bio).HasMaxLength(1000);
        builder.Property(x => x.UserId);
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.Property(x => x.ImagePath).HasMaxLength(300);
        builder.Property(x => x.CvPath).HasMaxLength(300);
    }
}
