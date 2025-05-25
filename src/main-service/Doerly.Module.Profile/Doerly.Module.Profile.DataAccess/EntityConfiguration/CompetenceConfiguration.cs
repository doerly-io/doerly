using Doerly.Module.Profile.DataAccess.Constants;
using Doerly.Module.Profile.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Profile.DataAccess.EntityConfiguration;

public class CompetenceConfiguration : IEntityTypeConfiguration<Competence>
{
    public void Configure(EntityTypeBuilder<Competence> builder)
    {
        builder.ToTable(DbConstants.Tables.Competence, DbConstants.ProfileSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CategoryId).IsRequired();
        builder.Property(x => x.CategoryName).IsRequired().HasMaxLength(100);
        
        builder.HasOne(x => x.Profile)
            .WithMany(p => p.Competences)
            .HasForeignKey(x => x.ProfileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(x => new { x.ProfileId, x.CategoryId }).IsUnique();
    }
}