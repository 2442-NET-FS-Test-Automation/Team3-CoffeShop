using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CoffeShop.Test.E2E.Pages
{
    /// <summary>
    /// Page Object de la pantalla de autenticación.
    ///
    /// NOTA DE LOCATORS: no fue posible inspeccionar el DOM real de
    /// CoffeShop.Frontend (GitHub bloqueó el acceso a los archivos del repo
    /// durante la generación de este proyecto). Los selectores usan la
    /// convención "data-testid", que es una práctica recomendada para pruebas
    /// automatizadas. Reemplázalos por los atributos/IDs reales del proyecto,
    /// o agrégalos al frontend si aún no existen.
    /// </summary>
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private By EmailInput => By.CssSelector("[data-testid='login-email']");
        private By PasswordInput => By.CssSelector("[data-testid='login-password']");
        private By SubmitButton => By.CssSelector("[data-testid='login-submit']");
        private By ErrorMessage => By.CssSelector("[data-testid='login-error']");

        public LoginPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public void NavigateTo(string baseUrl)
        {
            _driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}/login");
            _wait.Until(d => d.FindElement(EmailInput).Displayed);
        }

        public void Login(string email, string password)
        {
            var emailField = _driver.FindElement(EmailInput);
            emailField.Clear();
            emailField.SendKeys(email);

            var passwordField = _driver.FindElement(PasswordInput);
            passwordField.Clear();
            passwordField.SendKeys(password);

            _driver.FindElement(SubmitButton).Click();
        }

        public string GetErrorMessage()
        {
            return _wait.Until(d => d.FindElement(ErrorMessage)).Text;
        }
    }
}
