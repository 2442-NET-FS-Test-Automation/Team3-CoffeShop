using NUnit.Framework;
using CoffeShop.Test.E2E.Base;
using CoffeShop.Test.E2E.Config;
using CoffeShop.Test.E2E.Pages;

namespace CoffeShop.Test.E2E.Tests
{
    [TestFixture]
    public class TCQ22_PendingQueueTests : SeleniumTestBase
    {
        [Test]
        public void PendingQueue_OnlyShowsPaidOrders_IgnoresFulfilled()
        {
            var login = new LoginPage(Driver, Wait);
            login.NavigateTo(TestSettings.BaseUrl);
            login.Login(TestSettings.BaristaUsername, TestSettings.BaristaPassword);

            var queue = new BaristaQueuePage(Driver, Wait);
            queue.NavigateTo(TestSettings.BaseUrl);

            var statuses = queue.GetVisibleOrderStatuses();

            NUnit.Framework.Assert.That(statuses, Is.Not.Empty,
                "Se esperaba al menos un pedido 'Paid' precargado (seed) en la cola.");
            NUnit.Framework.Assert.That(statuses, Has.All.EqualTo("Paid"),
                "La cola de pendientes del Barista solo debe mostrar pedidos en estado 'Paid'.");
            NUnit.Framework.Assert.That(statuses, Has.None.EqualTo("Fulfilled"),
                "Los pedidos ya marcados como 'Fulfilled' no deben aparecer en la cola de pendientes.");
        }
    }
}