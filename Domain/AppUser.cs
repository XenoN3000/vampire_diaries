using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser: IdentityUser
{
    public string DeviceId { get; set; }

    public ICollection<Domain.Task> Diaries { get; set; } = new List<Task>();
    public ICollection<UserLogInTime> UserLogIns { get; set; } = new List<UserLogInTime>();
}