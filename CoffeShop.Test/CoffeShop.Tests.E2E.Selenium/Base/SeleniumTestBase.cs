using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CoffeShop.Test.E2E.Base
{
    /// <summary>
    /// Clase base para todos los tests E2E. Encapsula la inicialización y el cierre
    /// de ChromeDriver, además de un WebDriverWait reutilizable con espera explícita.
    /// </summary>
    public abstract class SeleniumTestBase
    {
        protected IWebDriver Driver = null!;
        protected WebDriverWait Wait = null!;

        [SetUp]
        public void SetUpDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            // Descomentar para ejecutar en modo headless (por ejemplo, en un pipeline de CI):
            // options.AddArgument("--headless=new");

            Driver = new ChromeDriver(options);
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Config.TestSettings.DefaultTimeoutSeconds));
        }

        [TearDown]
        public void TearDownDriver()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                TryTakeScreenshot(TestContext.CurrentContext.Test.Name);
            }

            Driver?.Quit();
            Driver?.Dispose();
        }

        private void TryTakeScreenshot(string testName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                var dir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots");
                Directory.CreateDirectory(dir);
                var safeName = string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));
                var path = Path.Combine(dir, $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(path);
                TestContext.AddTestAttachment(path, "Screenshot en el momento del fallo");
            }
            catch
            {
                // La captura de pantalla es un best-effort: nunca debe ocultar el motivo real del fallo.
            }
        }
    }
}
