using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Domain.Task> Tasks { get; set; }

    public DbSet<UserLogInTime> UserLogInsTime { get; set; }

    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Project>()
            .HasOne(u => u.Owner)
            .WithMany(d => d.Projects)
            .HasForeignKey(fk => fk.OwnerId);

        builder.Entity<UserLogInTime>()
            .HasKey(pk => new { DeviceId = pk.UserId, pk.LoggedInAt });

        builder.Entity<UserLogInTime>()
            .HasOne(u => u.User)
            .WithMany(l => l.UserLogIns)
            .HasForeignKey(fk => fk.UserId);


        builder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(fk => fk.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}