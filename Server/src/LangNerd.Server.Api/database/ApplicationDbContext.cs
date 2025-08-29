using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LangNerd.Server.Api.Models;
namespace LangNerd.Server.Api.Database;
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<WordModel> Words { get; set; }
    public DbSet<WordDefinition> WordDefinitions { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<WordModel>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.HasMany(w => w.Definitions)
                .WithOne()
                .HasForeignKey(d => d.WordId)
                .IsRequired();
        });
        builder.Entity<WordDefinition>(entity =>
        {
            entity.HasKey(w => w.Id);
        });
    }
}

