namespace Doerly.Module.Profile.DataAccess.Constants;

internal class DbConstants
{
    internal const string ProfileSchema = "profile";

    internal class Tables
    {
        internal const string Profile = "profile";
        internal const string Language = "language";
        internal const string LanguageProficiency = "language_proficiency";
        internal const string Review = "review";

        internal class ReviewTableConstraints
        {
            internal const string ReviewRatingRange = "ck_review_rating_range";
            internal const string ReviewReviewerNotReviewee = "ck_review_reviewer_not_reviewee";
        }
    }
}
