using Doerly.Module.Profile.DataAccess.Constants;
using Doerly.Module.Profile.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Profile.DataAccess.EntityConfiguration;

public class ReviewEntityConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(DbConstants.Tables.Review, DbConstants.ProfileSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Rating).IsRequired();
        builder.Property(x => x.Comment).HasMaxLength(2000);
        builder.Property(x => x.ReviewerUserId).IsRequired();
        builder.Property(x => x.ProfileId).IsRequired();

        builder.HasOne(x => x.Profile)
            .WithMany(p => p.ReviewsReceived)
            .HasForeignKey(x => x.ProfileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ReviewerProfile)
            .WithMany(p => p.ReviewsWritten)
            .HasForeignKey(x => x.ReviewerUserId)
            .IsRequired()
            .HasPrincipalKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ProfileId, x.DateCreated, x.Id }).IsUnique();
        builder.HasIndex(x => new { x.Id, x.ReviewerUserId });

        builder.ToTable(t =>
            t.HasCheckConstraint(DbConstants.Tables.ReviewTableConstraints.ReviewRatingRange, "rating >= 1 AND rating <= 5"));
    }
}
