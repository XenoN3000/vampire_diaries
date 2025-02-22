using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser: IdentityUser
{
    public string DeviceId { get; set; }

    public ICollection<Diary> Diaries { get; set; } = new List<Diary>();
}