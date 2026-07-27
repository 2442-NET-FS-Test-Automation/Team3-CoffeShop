import { use, useEffect, useState } from "react";
import { api } from "../api/client";
import "./Inventorypage.css";
import type { InventoryItem } from "../api/inventory";
import { useAuth } from "../auth/useAuth";

export interface Product 
{
    ProductId: number;
    name: string;
    price: number;
    stock: number;
    soldToday: number;

}
const InventoryPage = () => {

    const { user } = useAuth();
    console.log(user);
    const userRole = user?.role || "Barista";
    const [products, setProducts] = useState<InventoryItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        fetchInventory();
    }, [])

    const fetchInventory = async () => {
            setIsLoading(true);
        try
        {
            const response = await api.get("/api/inventory");
            setProducts(response.data);
            setError(null);

        } 
        catch(err)
        {
            console.error("Error loading the inventory", err);
            setError("There was an issue with the inventory");
        }
        finally
        {
            setIsLoading(false);
        }
    };

    const totalSKUs = products?.length;
    const lowStock = products?.filter(p => p.stock < 10).length;


    return (
        <div className="inventory-wrapper">
            
            <div className="inventory-metrics">
                <div className="metric-card">
                    <h3 className="metric-gold">{totalSKUs}</h3>
                    <p className="metric-label">Total SKUs</p>
                </div>
                <div className={`metric-card ${lowStock > 0 ? 'metric-alert' : ''}`}>
                    <h3 className="metric-red">{lowStock}</h3>
                    <p className="metric-label">Low Stock Items</p>
                </div>
            </div>

            <div className="inventory-container">
                <div className="inventory-header">
                    <h2>Inventory Overview</h2>
                    {userRole === 'Manager' && <button className="add-btn">+ Add New Product</button> }
                </div>

                {error && <p className="error-text">{error}</p>}

                {isLoading ? (
                    <p className="loading-text">Loading inventory from database...</p>
                ) : (
                    <table className="inventory-table">
                        <thead>
                            <tr>
                                <th>SKU</th>
                                <th>ITEM</th>
                                <th>CATEGORY</th>
                                <th>PRICE</th>
                                <th>IN STOCK</th>
                                { userRole === 'Manager' && <th className="text-right">ACTIONS</th> }
                            </tr>
                        </thead>
                        <tbody>
                            {products.length === 0 ? (
                                <tr>
                                    <td colSpan={userRole === 'Manager' ? 6 : 5} className="empty-row">
                                        No hay productos en el inventario.
                                    </td>
                                </tr>
                            ) : (

                                products.map((product) => (
                                    <tr key={product.sku}>

                                        <td>
                                            {product.sku}
                                        </td>
                                        <td className="item-name">
                                            {product.name}
                                            </td>

                                        <td>
                                            <span className="category-badge">
                                                General
                                            </span>
                                        </td>

                                        <td>
                                            ${product.price ? product.price.toFixed(2) : "0.00"}
                                        </td >

                                        <td className={product.stock < 10 ? 'text-red' : 'text-green'}>
                                            {product.stock}
                                        </td>

                                        {userRole === 'Manager' && (
                                            <td className="text-right">
                                                <button className="action-btn"> Edit </button>
                                        </td>
                                        )}
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                )}
            </div>
        </div>
    );
};

export default InventoryPage;
