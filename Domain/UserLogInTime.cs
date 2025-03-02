namespace Domain;

public class UserLogInTime
{
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public DateTime LoggedInAt { get; set; }
}