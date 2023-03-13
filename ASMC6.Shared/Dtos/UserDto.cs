namespace ASMC6.Shared.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Descreption { get; set; }
    public string Image { get; set; }
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;


}