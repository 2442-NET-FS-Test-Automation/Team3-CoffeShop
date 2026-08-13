using NUnit.Framework;
using CoffeShop.Test.E2E.Base;
using CoffeShop.Test.E2E.Config;
using CoffeShop.Test.E2E.Pages;
using NUnit.Framework.Legacy;

namespace CoffeShop.Test.E2E.Tests
{
    [TestFixture]
    public class TCQ26_AdminRoleUpdateTests : SeleniumTestBase
    {
        [Test]
        public void Admin_CanUpdateUserRole_AndChangeIsPersisted()
        {
            var login = new LoginPage(Driver, Wait);
            login.NavigateTo(TestSettings.BaseUrl);
            login.Login(TestSettings.AdminUsername, TestSettings.AdminPassword);

            var roleMgmt = new RoleManagementPage(Driver, Wait);
            roleMgmt.NavigateTo(TestSettings.BaseUrl);

            roleMgmt.ChangeUserRole(TestSettings.TargetUserEmail, "Cashier");

            StringAssert.Contains("updated", roleMgmt.GetSuccessMessage().ToLowerInvariant());

            // Se recarga la pantalla para confirmar que el cambio quedó persistido
            // en la base de datos, y no solo en el estado en memoria de la UI.
            roleMgmt.NavigateTo(TestSettings.BaseUrl);
            NUnit.Framework.Assert.That(roleMgmt.GetCurrentRoleDisplayed(TestSettings.TargetUserEmail), Is.EqualTo("Cashier"),
                "El cambio de rol debe persistir en la base de datos y reflejarse tras recargar la página.");
        }
    }

}