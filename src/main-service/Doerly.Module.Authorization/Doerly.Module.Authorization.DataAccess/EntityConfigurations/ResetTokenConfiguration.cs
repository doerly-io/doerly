using Doerly.Module.Authorization.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Authorization.DataAccess.EntityConfigurations;

public class ResetTokenConfiguration : IEntityTypeConfiguration<ResetToken>
{
    public void Configure(EntityTypeBuilder<ResetToken> builder)
    {
        builder.HasKey(x => x.Guid);
        builder.Property(x => x.DateCreated).IsRequired();
        builder.HasOne(x => x.User).WithMany(x => x.ResetTokens).HasForeignKey(x => x.UserId);
        
    }
}
