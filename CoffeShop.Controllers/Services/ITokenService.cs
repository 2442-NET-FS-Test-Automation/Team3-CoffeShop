namespace CoffeShop.Controllers.Services;

public interface ITokenService
{
    string Issue(string user, string role);
}