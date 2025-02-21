using Domain;

namespace API.DTOs;

public class UserDto
{
    public string DeviceId { get; set; }
    public string Token { get; set; }

    
    
    public static UserDto CreateFromUser(AppUser user, string token) => new UserDto
    {
        DeviceId = user.DeviceId,
        Token = token
    };
    
    public static UserDto CreateFromUserDto(UserDto user) => new UserDto
    {
        DeviceId = user.DeviceId,
        Token = user.Token
    };
}