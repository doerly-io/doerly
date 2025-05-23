using Doerly.Module.Profile.DataAccess.Constants;
using Doerly.Module.Profile.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Profile.DataAccess.EntityConfiguration;

public class LanguageProficiencyConfiguration : IEntityTypeConfiguration<LanguageProficiency>
{
    public void Configure(EntityTypeBuilder<LanguageProficiency> builder)
    {
        builder.ToTable(DbConstants.Tables.LanguageProficiency, DbConstants.ProfileSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Level).IsRequired();
        
        builder.HasOne(x => x.Language)
            .WithMany(l => l.LanguageProficiencies)
            .HasForeignKey(x => x.LanguageId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.Profile)
            .WithMany(p => p.LanguageProficiencies)
            .HasForeignKey(x => x.ProfileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(x => new { x.ProfileId, x.LanguageId }).IsUnique();
    }
}
