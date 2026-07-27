

const DashboardPage = () => { 




    return( 
        <div className="dashboard-wrapper">
            <div className="dashboard-header">
                <h1>Coffee Shop Analytics</h1>
                <p>Resumen general del inventario</p>
            </div>

            <div className="stats-grid">
                {/* Card 1 */}
                <div className="stat-card">
                    <span className="stat-title">Total Products</span>
                    <span className="stat-value">{stats.totalProducts}</span>
                    <span className="stat-desc">Items registrados en el sistema</span>
                </div>

                {/* Card 2 */}
                <div className="stat-card">
                    <span className="stat-title">Low Stock Alerts</span>
                    <span className={`stat-value ${stats.lowStockItems > 0 ? 'text-danger' : 'text-success'}`}>
                        {stats.lowStockItems}
                    </span>
                    <span className="stat-desc">Productos con menos de 10 unidades</span>
                </div>

                {/* Tarjeta 3 */}
                <div className="stat-card">
                    <span className="stat-title">Categories</span>
                    <span className="stat-value">{stats.totalCategories}</span>
                    <span className="stat-desc">Clasificaciones activas</span>
                </div>

                {/* Tarjeta 4 */}
                <div className="stat-card">
                    <span className="stat-title">Total Stock Value</span>
                    <span className="stat-value">${stats.totalStockValue.toFixed(2)}</span>
                    <span className="stat-desc">Capital invertido en inventario</span>
                </div>
            </div>
        </div>

    );
};