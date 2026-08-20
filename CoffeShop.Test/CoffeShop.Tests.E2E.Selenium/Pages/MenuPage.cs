using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using CoffeShop.Test.E2E.Config;

namespace CoffeShop.Test.E2E.Pages;

public class MenuPage
{
    private const string MockConsumerFlowScript = """
(() => {
    const fakeBaristaToken =
        "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJCYXJpc3RhIn0.signature";

    const inventory = [
        { productId: 1, sku: "HOT-AME-01", name: "American", stock: 5, price: 50 },
        { productId: 2, sku: "HOT-LAT-02", name: "Latte", stock: 4, price: 65 },
    ];

    const order = {
        orderId: 123,
        userId: 1,
        cashierName: "Admin",
        total: 50,
        lines: [
            {
                orderLineId: 1,
                productId: 1,
                productName: "American",
                quantity: 1,
                unitPrice: 50,
                subtotal: 50,
            },
        ],
    };

    window.localStorage.setItem("Access.Token", fakeBaristaToken);

    try {
        Object.defineProperty(window, "XMLHttpRequest", {
            value: undefined,
            configurable: true,
        });
    } catch {
        window.XMLHttpRequest = undefined;
    }

    const originalFetch = window.fetch ? window.fetch.bind(window) : undefined;

    window.fetch = async (input, init = {}) => {
        const url = typeof input === "string" ? input : input.url;
        const method = (init.method || input.method || "GET").toUpperCase();

        if (url.includes("/api/inventory") && method === "GET") {
            return new Response(JSON.stringify(inventory), {
                status: 200,
                headers: { "Content-Type": "application/json" },
            });
        }

        if (url.includes("/api/orders") && method === "POST") {
            return new Response(JSON.stringify(order), {
                status: 201,
                headers: { "Content-Type": "application/json" },
            });
        }

        if (originalFetch) return originalFetch(input, init);
        throw new Error(`No mock for ${method} ${url}`);
    };
})();
""";

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public MenuPage(IWebDriver driver, WebDriverWait wait)
    {
        _driver = driver;
        _wait = wait;
    }

    public string Url => _driver.Url;

    public MenuPage OpenWithMockedConsumerFlow(string? baseUrl = null)
    {
        if (_driver is not ChromeDriver chromeDriver)
        {
            throw new NotSupportedException("El flujo con mocks requiere ChromeDriver para registrar el script CDP.");
        }

        chromeDriver.ExecuteCdpCommand(
            "Page.addScriptToEvaluateOnNewDocument",
            new Dictionary<string, object?> { ["source"] = MockConsumerFlowScript });

        var targetUrl = baseUrl ?? TestSettings.BaseUrl;
        _driver.Navigate().GoToUrl($"{targetUrl.TrimEnd('/')}/menu");
        WaitForText("Menu");
        WaitForText("American");

        return this;
    }

    public MenuPage AddAmericanToCart()
    {
        WaitFor(By.XPath("//div[contains(@class,'profile-card')][.//h2[contains(.,'Name: American')]]//button[normalize-space()='Add to cart']")).Click();
        return this;
    }

    public MenuPage Checkout()
    {
        WaitFor(By.CssSelector(".cart-checkout")).Click();
        return this;
    }

    public string WaitForCartToContain(string text)
    {
        return _wait.Until(driver =>
        {
            var cart = driver.FindElement(By.CssSelector(".cart-panel"));
            return cart.Text.Contains(text, StringComparison.OrdinalIgnoreCase) ? cart.Text : null;
        })!;
    }

    public string WaitForSuccessMessage()
    {
        return WaitFor(By.CssSelector(".cart-status-success")).Text;
    }

    private IWebElement WaitFor(By by)
    {
        return _wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(by);
                return element.Displayed ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        })!;
    }

    private void WaitForText(string text)
    {
        _wait.Until(driver => driver.PageSource.Contains(text, StringComparison.OrdinalIgnoreCase));
    }
}
