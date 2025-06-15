using Doerly.Module.Profile.DataAccess.Constants;
using Doerly.Module.Profile.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doerly.Module.Profile.DataAccess.EntityConfiguration;

public class FeedbackEntityConfiguration : IEntityTypeConfiguration<FeedbackEntity>
{
    public void Configure(EntityTypeBuilder<FeedbackEntity> builder)
    {
        builder.ToTable(DbConstants.Tables.Feedback, DbConstants.ProfileSchema);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Comment).HasMaxLength(2000);
        builder.Property(x => x.Rating).IsRequired();

        builder.HasOne(x => x.ReviewerProfile)
            .WithMany(x => x.FeedbackGiven)
            .HasForeignKey(x => x.ReviewerUserId)
            .HasPrincipalKey(x => x.UserId);

        builder.HasOne(x => x.RevieweeProfile)
            .WithMany(x => x.FeedbackReceived)
            .HasForeignKey(x => x.RevieweeUserId)
            .HasPrincipalKey(x => x.UserId);
        
        builder.HasIndex(x => x.RevieweeUserId);
        builder.HasIndex(x => x.LastModifiedDate);
        
        builder.ToTable(t =>
            t.HasCheckConstraint("ck_feedback_rating_range", "\"rating\" >= 1 AND \"rating\" <= 5"));

    }
}