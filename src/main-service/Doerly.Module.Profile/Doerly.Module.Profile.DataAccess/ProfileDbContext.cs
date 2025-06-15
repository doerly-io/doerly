using Doerly.DataAccess;
using Doerly.Module.Profile.DataAccess.Constants;
using Doerly.Module.Profile.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Profile.DataAccess;

public class ProfileDbContext(IConfiguration configuration) : BaseDbContext(configuration)
{
    public DbSet<Models.Profile> Profiles { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<LanguageProficiency> LanguageProficiencies { get; set; }
    public DbSet<Competence> Competences { get; set; }

    public DbSet<FeedbackEntity> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfileDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DbConstants.ProfileSchema);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["ConnectionStrings:ProfileConnection"]);

        base.OnConfiguring(optionsBuilder);
    }
}
