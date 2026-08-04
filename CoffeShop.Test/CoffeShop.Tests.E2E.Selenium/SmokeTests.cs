using Xunit;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
        _driver.Url.Should().Contain("/login");
        _driver.FindElement(By.TagName("h2")).Text.Should().Be("The best coffee of Cognizant");
        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
        submitButton.Text.Should().Be("Submit");
        _driver.FindElement(By.CssSelector("input[type='password']")).GetAttribute("type").Should().Be("password");
    }
    [Fact]
    public void LoginPage_HasUsernameAndPasswordInputs()
    {
        var usernameInput = _driver.FindElement(By.Id("username"));
        usernameInput.GetAttribute("name").Should().Be("username");
        usernameInput.GetAttribute("placeholder").Should().Be("Enter your username");
    }
}
