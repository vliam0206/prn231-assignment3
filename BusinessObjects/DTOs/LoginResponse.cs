namespace BusinessObjects.DTOs;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public int AccountId { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
    public byte Status { get; set; } = 1;
}
