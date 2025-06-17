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
        builder.Property(x => x.Rating).HasPrecision(3, 2);
        
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.HasIndex(x => x.Rating);
        
        builder.ToTable(t =>
            t.HasCheckConstraint("ck_profile_rating_range", "\"rating\" >= 1 AND \"rating\" <= 5"));
    }
}
