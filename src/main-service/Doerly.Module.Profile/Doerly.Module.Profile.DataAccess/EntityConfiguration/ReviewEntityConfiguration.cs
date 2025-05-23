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
        builder.Property(x => x.ReviewerId).IsRequired();
        builder.Property(x => x.RevieweeId).IsRequired();
        builder.HasOne(x => x.Reviewer)
            .WithMany(p => p.ReviewsWritten)
            .HasForeignKey(x => x.ReviewerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Reviewee)
            .WithMany(p => p.ReviewsReceived)
            .HasForeignKey(x => x.RevieweeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.ToTable(t => t.HasCheckConstraint(DbConstants.Tables.ReviewTableConstraints.ReviewRatingRange, "rating >= 1 AND rating <= 5"));
        builder.ToTable(t => t.HasCheckConstraint(DbConstants.Tables.ReviewTableConstraints.ReviewReviewerNotReviewee, "reviewer_id >= 1 AND reviewee_id <= 5"));

    }
}
