using OpenQA.Selenium;

namespace CoffeShop.Tests.E2E.Selenium.Pages;


public class LoginPage
{
    private readonly IWebDriver _driver;

    // Doesn't save a fixed Url. Every call reads a current Url
    public string Url => _driver.Url;

    public string HeadingText => _driver.FindElement(By.TagName("h2")).Text;
    public string SubmitButtonText => _driver.FindElement(By.CssSelector("button[type='submit']")).Text;

    public string? PasswordInputType => _driver.FindElement(By.CssSelector("input[type='password']")).GetAttribute("type");
    public string ErrorMessageText => _driver.FindElement(By.CssSelector(".floating-error-bubble")).Text;

    //Constructor
    public LoginPage(IWebDriver driver){ _driver = driver; }

    public LoginPage Open()
    {
        _driver.Navigate().GoToUrl("http://localhost:5173/");
        return this;
    }

    public LoginPage TypeUsername(string username)
    {
        var usernameInput = _driver.FindElement(By.Id("username"));
        usernameInput.Clear();
        usernameInput.SendKeys(username);
        return this;
    }
    public LoginPage TypePassword(string password)
    {
        var passwordInput = _driver.FindElement(By.Id("password"));
        passwordInput.Clear();
        passwordInput.SendKeys(password);
        return this;
    }
    public LoginPage Submit()
    {
        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
        submitButton.Click();
        return this;
    }
    public LoginPage LoginAs(string username, string password)
    {   
        TypeUsername(username);
        TypePassword(password);
        Submit();
        return this;
    }
}
