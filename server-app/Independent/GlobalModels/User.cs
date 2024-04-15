namespace Agnostics.GlobalModels;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Password { get; set; }
    public string? Salt { get; set; }
}