using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser: IdentityUser
{
    public string DeviceId { get; set; }
}