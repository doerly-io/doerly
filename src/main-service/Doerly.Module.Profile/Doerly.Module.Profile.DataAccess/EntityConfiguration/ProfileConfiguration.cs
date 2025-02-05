using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfileModel = Doerly.Module.Profile.DataAccess.Models.Profile;

namespace Doerly.Module.Profile.DataAccess.EntityConfiguration;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileModel>
{
    public void Configure(EntityTypeBuilder<ProfileModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();
        builder.Property(x => x.Patronymic);
        builder.Property(x => x.DateOfBirth);
        builder.Property(x => x.Sex);
        builder.Property(x => x.Bio);
        
    }
}