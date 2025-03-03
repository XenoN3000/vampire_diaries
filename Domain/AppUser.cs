using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser: IdentityUser
{
    public string DeviceId { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<UserLogInTime> UserLogIns { get; set; } = new List<UserLogInTime>();
}