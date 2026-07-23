namespace CoffeShop.Controllers.DTOs;

public class UserDto
{
    
    public int Id {get; set;}
    public string Name {get; set;}
    public string Username {get; set;}
    public string Email {get; set;}
    public string Role {get; set;}

}

public class UserCreateDto
{

    public string Username {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
    public string Role {get; set;}
}

public class UserUpdateDto
{

    public string Username {get; set;}
    public string Email {get; set;}
    public string Name {get; set;}
    public string Role {get; set;}
}