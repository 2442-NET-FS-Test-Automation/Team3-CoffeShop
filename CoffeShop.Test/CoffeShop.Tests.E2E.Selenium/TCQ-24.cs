using NUnit.Framework;
using CoffeShop.Test.E2E.Base;
using CoffeShop.Test.E2E.Config;
using CoffeShop.Test.E2E.Pages;

namespace CoffeShop.Test.E2E.Tests
{
    [TestFixture]
    public class TCQ24_FulfillOrderTests : SeleniumTestBase
    {
        // TODO: reemplazar por un Order Id real generado por el seed de datos de prueba.
        private const string SeededPaidOrderId = "ORD-1001";

        [Test]
        public void CompletingOrder_RemovesItFromActiveQueue()
        {
            var login = new LoginPage(Driver, Wait);
            login.NavigateTo(TestSettings.BaseUrl);
            login.Login(TestSettings.BaristaUsername, TestSettings.BaristaPassword);

            var queue = new BaristaQueuePage(Driver, Wait);
            queue.NavigateTo(TestSettings.BaseUrl);

            NUnit.Framework.Assert.That(queue.IsOrderVisible(SeededPaidOrderId), Is.True,
                $"Precondición no cumplida: el pedido {SeededPaidOrderId} no se encontró en estado 'Paid'.");

            queue.CompleteOrder(SeededPaidOrderId);
            queue.WaitUntilOrderDisappears(SeededPaidOrderId);

            NUnit.Framework.Assert.That(queue.IsOrderVisible(SeededPaidOrderId), Is.False,
                "El pedido debe desaparecer de la cola activa una vez marcado como 'Fulfilled'.");
        }
    }
}