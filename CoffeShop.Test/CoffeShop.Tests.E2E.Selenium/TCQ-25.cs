using NUnit.Framework;
using CoffeShop.Test.E2E.Base;
using CoffeShop.Test.E2E.Config;
using CoffeShop.Test.E2E.Pages;

namespace CoffeShop.Test.E2E.Tests
{
    [TestFixture]
    public class TCQ25_CashierRoleAccessTests : SeleniumTestBase
    {
        [Test]
        public void Cashier_CannotAccessUserRoleManagement()
        {
            var login = new LoginPage(Driver, Wait);
            login.NavigateTo(TestSettings.BaseUrl);
            login.Login(TestSettings.CashierUsername, TestSettings.CashierPassword);

            var roleMgmt = new RoleManagementPage(Driver, Wait);
            roleMgmt.NavigateTo(TestSettings.BaseUrl);

            NUnit.Framework.Assert.That(roleMgmt.IsAccessDenied(), Is.True,
                "Un usuario Cashier que navega a la pantalla de gestión de roles debe ser bloqueado " +
                "(403 / redirección), y nunca debe poder modificar roles de usuario.");
        }
    }
}
