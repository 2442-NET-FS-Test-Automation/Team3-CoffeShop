using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CoffeShop.Test.E2E.Pages
{
    /// <summary>
    /// Page Object de la cola de pedidos pendientes del Barista (US5.1 / US5.2).
    /// Ver nota de locators en LoginPage.cs.
    /// </summary>
    public class BaristaQueuePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private By QueueContainer => By.CssSelector("[data-testid='barista-queue']");
        private By OrderCards => By.CssSelector("[data-testid='order-card']");
        private By OrderStatusBadge => By.CssSelector("[data-testid='order-status']");
        private By OrderIdLabel => By.CssSelector("[data-testid='order-id']");
        private By CompleteButton => By.CssSelector("[data-testid='order-complete-button']");

        public BaristaQueuePage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public void NavigateTo(string baseUrl)
        {
            _driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}/barista/queue");
            _wait.Until(d => d.FindElement(QueueContainer).Displayed);
        }

        public IReadOnlyList<IWebElement> GetOrderCards() => _driver.FindElements(OrderCards);

        /// <summary>Devuelve el estado mostrado en cada tarjeta de pedido visible en la cola.</summary>
        public IReadOnlyList<string> GetVisibleOrderStatuses()
        {
            return GetOrderCards()
                .Select(card => card.FindElement(OrderStatusBadge).Text.Trim())
                .ToList();
        }

        public bool IsOrderVisible(string orderId)
        {
            return GetOrderCards()
                .Any(card => card.FindElement(OrderIdLabel).Text.Trim() == orderId);
        }

        public void CompleteOrder(string orderId)
        {
            var card = GetOrderCards()
                .First(c => c.FindElement(OrderIdLabel).Text.Trim() == orderId);
            card.FindElement(CompleteButton).Click();
        }

        /// <summary>Espera activa a que el pedido desaparezca de la cola tras marcarlo como completado.</summary>
        public void WaitUntilOrderDisappears(string orderId, int timeoutSeconds = 10)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(_ => !IsOrderVisible(orderId));
        }
    }
}
