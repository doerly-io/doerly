using Doerly.Module.Authorization.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Authorization.DataAccess.EntityConfigurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Guid).IsRequired();
        builder.HasOne(e => e.User).WithMany(e => e.RefreshTokens).HasForeignKey(e => e.UserId);
        
        builder.HasIndex(e => e.Guid).IsUnique();
    }
}
