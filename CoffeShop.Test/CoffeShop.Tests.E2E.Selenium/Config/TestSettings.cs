namespace CoffeShop.Test.E2E.Config
{
    /// <summary>
    /// Configuración centralizada del entorno de pruebas E2E.
    /// Los valores se leen de variables de entorno (recomendado para CI/CD)
    /// y caen a valores por defecto pensados para un entorno local de desarrollo.
    ///
    /// IMPORTANTE: los usuarios de prueba (Admin/Cashier/Barista) y el usuario
    /// objetivo para el cambio de rol (TargetUserEmail) deben existir previamente
    /// como datos "seed" en la base de datos de pruebas antes de ejecutar la suite.
    /// </summary>
    public static class TestSettings
    {
        public static string BaseUrl =>
            Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "https://localhost:5173";

        // Rol Admin (US6.1 - Manage Roles)
        public static string AdminUsername =>
            Environment.GetEnvironmentVariable("E2E_ADMIN_USER") ?? "admin.test@coffeeshop.com";
        public static string AdminPassword =>
            Environment.GetEnvironmentVariable("E2E_ADMIN_PASS") ?? "Admin#12345";

        // Rol Cashier (US6.1 - Auth Matrix)
        public static string CashierUsername =>
            Environment.GetEnvironmentVariable("E2E_CASHIER_USER") ?? "cashier.test@coffeeshop.com";
        public static string CashierPassword =>
            Environment.GetEnvironmentVariable("E2E_CASHIER_PASS") ?? "Cashier#12345";

        // Rol Barista (US5.1 / US5.2)
        public static string BaristaUsername =>
            Environment.GetEnvironmentVariable("E2E_BARISTA_USER") ?? "barista.test@coffeeshop.com";
        public static string BaristaPassword =>
            Environment.GetEnvironmentVariable("E2E_BARISTA_PASS") ?? "Barista#12345";

        // Usuario objetivo cuyo rol será modificado por el Admin (TCQ-26)
        public static string TargetUserEmail =>
            Environment.GetEnvironmentVariable("E2E_TARGET_USER") ?? "test.user@coffeeshop.com";

        public static int DefaultTimeoutSeconds => 10;
    }
}
