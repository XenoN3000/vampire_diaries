using System.Globalization;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserTask = Domain.Task;

namespace Persistence;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Domain.Task> Tasks { get; set; }

    public DbSet<UserLogInTime> UserLogInsTime { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AppUser>()
            .Property(u => u.EmailConfirmed)
            .HasConversion(b => b ? 1 : 0, i => i == 1);
        
        builder.Entity<AppUser>()
            .Property(u => u.LockoutEnabled)
            .HasConversion(b => b ? 1 : 0, i => i == 1);
        
        builder.Entity<AppUser>()
            .Property(u => u.PhoneNumberConfirmed)
            .HasConversion(b => b ? 1 : 0, i => i == 1);
        
        builder.Entity<AppUser>()
            .Property(u => u.TwoFactorEnabled)
            .HasConversion(b => b ? 1 : 0, i => i == 1);
        
        builder.Entity<UserLogInTime>()
            .Property(u => u.LoggedInAt)
            .HasConversion(d => d.ToString(), s => DateTime.Parse(s));
        
        builder.Entity<Domain.Task>()
            .Property(p => p.Id)
            .HasConversion(g => g.ToString(), s => Guid.Parse(s));
        
        builder.Entity<Domain.Task>()
            .Property(u => u.StartTim)
            .HasConversion(d => d.ToString(), s => DateTime.Parse(s));
        
        builder.Entity<Domain.Task>()
            .Property(u => u.Duration)
            .HasConversion(d => d.ToString(), s => DateTime.Parse(s));




        builder.Entity<UserTask>()
            .HasOne(u => u.Owner)
            .WithMany(d => d.Tasks)
            .HasForeignKey(fk => fk.OwnerId);
        

        builder.Entity<UserLogInTime>()
            .HasKey(pk => new { DeviceId = pk.UserId, pk.LoggedInAt });

        builder.Entity<UserLogInTime>()
            .HasOne(u => u.User)
            .WithMany(l => l.UserLogIns)
            .HasForeignKey(fk => fk.UserId);


   
    }
}