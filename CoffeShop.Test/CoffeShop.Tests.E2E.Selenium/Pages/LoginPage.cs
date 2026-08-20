using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CoffeShop.Test.E2E.Pages
{
    /// <summary>
    /// Page Object de la pantalla de autenticación.
    /// Los selectores reflejan los IDs y clases publicados por el frontend.
    /// </summary>
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private static readonly By UsernameInput = By.Id("username");
        private static readonly By PasswordInput = By.Id("password");
        private static readonly By SubmitButton = By.CssSelector("button[type='submit']");
        private static readonly By ErrorMessage = By.CssSelector(".floating-error-bubble");

        public LoginPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public void NavigateTo(string baseUrl)
        {
            _driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}/login");
            WaitForVisible(UsernameInput);
        }

        public void Login(string username, string password)
        {
            var usernameField = WaitForVisible(UsernameInput);
            usernameField.Clear();
            usernameField.SendKeys(username);

            var passwordField = WaitForVisible(PasswordInput);
            passwordField.Clear();
            passwordField.SendKeys(password);

            WaitForVisible(SubmitButton).Click();
        }

        public string GetErrorMessage()
        {
            return WaitForVisible(ErrorMessage).Text;
        }

        private IWebElement WaitForVisible(By locator)
        {
            return _wait.Until(driver =>
            {
                var elements = driver.FindElements(locator);
                return elements.FirstOrDefault(element => element.Displayed);
            })!;
        }
    }
}
