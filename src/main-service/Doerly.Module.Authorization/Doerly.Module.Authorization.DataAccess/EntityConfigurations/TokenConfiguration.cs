using Doerly.Module.Authorization.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Authorization.DataAccess.EntityConfigurations;

public class TokenConfiguration : IEntityTypeConfiguration<TokenEntity>
{
    public void Configure(EntityTypeBuilder<TokenEntity> builder)
    {
        builder.HasKey(x => x.Guid);
        builder.Property(x => x.DateCreated).IsRequired();
        builder.Property(x => x.TokenKind).IsRequired().HasConversion<byte>();
        builder.Property(x => x.Value).IsRequired().HasMaxLength(44).IsFixedLength();
        builder.HasOne(x => x.User).WithMany(x => x.Tokens).HasForeignKey(x => x.UserId);

        builder.HasIndex(x => new { x.UserId, x.TokenKind });
        builder.HasIndex(x => x.Value).HasMethod("hash");
    }
}
