using Xunit;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using CoffeShop.Tests.E2E.Selenium.Pages;

namespace CoffeShop.Tests.E2E.Selenium;

public class SmokeTests : IDisposable
{
    private readonly ChromeDriver _driver;
    public SmokeTests()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--window-size=1280,900");

        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        _driver.Navigate().GoToUrl("http://localhost:5173/");
    }

    public void Dispose()
    {
     _driver.Quit();   
    }
    [Fact]
    public void OpeningTheSpa_RedirectsToLogin()
    {
        var loginPage = new LoginPage(_driver);
        loginPage.Open();
        loginPage.Url.Should().Contain("login");
        loginPage.HeadingText.Should().Be("The best coffee of Cognizant");
        loginPage.SubmitButtonText.Should().Be("Submit");
        loginPage.PasswordInputType.Should().Be("password");
    }
    [Fact]
    public void LoginPage_HasUsernameAndPasswordInputs()
    {
        var usernameInput = _driver.FindElement(By.Id("username"));
        usernameInput.GetAttribute("name").Should().Be("username");
        usernameInput.GetAttribute("placeholder").Should().Be("Enter your username");
    }
    [Fact]
    public void Login_WithInvalidCredentials_StaysOnLoginPage()
    {
        //Arrange
        var loginPage = new LoginPage(_driver);
        loginPage.Open();

        //Act
        loginPage.LoginAs("baduser", "badpass");

        //Assert
        loginPage.Url.Should().Contain("login");
        loginPage.ErrorMessageText.Should().Be("User or password are not valid.");
    }
}
