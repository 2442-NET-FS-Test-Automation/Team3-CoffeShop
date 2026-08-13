using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CoffeShop.Test.E2E.Pages
{
    /// <summary>
    /// Page Object de la pantalla de administración de usuarios/roles (US6.1).
    /// Ver nota de locators en LoginPage.cs.
    /// </summary>
    public class RoleManagementPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private By RoleSelect(string userEmail) => By.CssSelector($"[data-testid='role-select-{userEmail}']");
        private By SaveRoleButton(string userEmail) => By.CssSelector($"[data-testid='role-save-{userEmail}']");
        private By SuccessToast => By.CssSelector("[data-testid='toast-success']");
        private By ForbiddenBanner => By.CssSelector("[data-testid='forbidden-banner']");

        public RoleManagementPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public void NavigateTo(string baseUrl)
        {
            _driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}/admin/users");
        }

        /// <summary>
        /// True si el acceso fue bloqueado: ya sea por un banner de 403 renderizado
        /// en la UI, o por una redirección a /login o /unauthorized.
        /// </summary>
        public bool IsAccessDenied()
        {
            try
            {
                _wait.Until(d =>
                    d.FindElements(ForbiddenBanner).Any(e => e.Displayed)
                    || d.Url.Contains("/unauthorized")
                    || d.Url.Contains("/login"));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void ChangeUserRole(string userEmail, string newRole)
        {
            var select = new SelectElement(_driver.FindElement(RoleSelect(userEmail)));
            select.SelectByText(newRole);
            _driver.FindElement(SaveRoleButton(userEmail)).Click();
        }

        public string GetSuccessMessage()
        {
            return _wait.Until(d => d.FindElement(SuccessToast)).Text;
        }

        public string GetCurrentRoleDisplayed(string userEmail)
        {
            var select = new SelectElement(_wait.Until(d => d.FindElement(RoleSelect(userEmail))));
            return select.SelectedOption.Text.Trim();
        }
    }
}
