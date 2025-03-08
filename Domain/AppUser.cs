using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser: IdentityUser
{
    public string DeviceId { get; set; }
    public ICollection<Domain.Task> Tasks { get; set; } = new List<Domain.Task>();
    public ICollection<UserLogInTime> UserLogIns { get; set; } = new List<UserLogInTime>();
}