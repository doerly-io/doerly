using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Profile.DataAccess.EntityConfiguration;

public class ProfileConfiguration : IEntityTypeConfiguration<Models.Profile>
{
    public void Configure(EntityTypeBuilder<Models.Profile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.DateOfBirth);
        builder.Property(x => x.Sex);
        builder.Property(x => x.Bio).HasMaxLength(1000);
        builder.Property(x => x.UserId);
        builder.Property(x => x.ImagePath).HasMaxLength(300);
        builder.Property(x => x.CvPath).HasMaxLength(300);
        builder.Property(x => x.CityId);

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => x.UserId).IsUnique();
    }
}
