using CoffeShop.Test.E2E.Base;
using CoffeShop.Test.E2E.Config;
using CoffeShop.Test.E2E.Pages;
using NUnit.Framework;

namespace CoffeShop.Test.E2E.Tests
{
    [TestFixture]
    public class TCQ05_ConsumerFlowTests : SeleniumTestBase
    {
        [Test]
        public void Consumer_CanBrowseMenuAddItemAndCreateOrder()
        {
            var menu = new MenuPage(Driver, Wait);

            menu.OpenWithMockedConsumerFlow(TestSettings.BaseUrl);
            menu.AddAmericanToCart();
            var cartText = menu.WaitForCartToContain("American");
            menu.Checkout();
            var successMessage = menu.WaitForSuccessMessage();

            NUnit.Framework.Assert.Multiple(() =>
            {
                NUnit.Framework.Assert.That(menu.Url, Does.Contain("/menu"));
                NUnit.Framework.Assert.That(cartText, Does.Contain("American"));
                NUnit.Framework.Assert.That(cartText, Does.Contain("$50"));
                NUnit.Framework.Assert.That(successMessage, Does.Contain("Order #123 created"));
                NUnit.Framework.Assert.That(successMessage, Does.Contain("Total: $50.00"));
            });
        }
    }
}
