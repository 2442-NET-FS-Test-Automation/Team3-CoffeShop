import { useEffect, useState } from "react";
import { api } from "../api/client";
import "./Inventorypage.css";
import { editDrink, type InventoryItem } from "../api/inventory";
import { useAuth } from "../auth/useAuth";

const normalizeStock = (stock: number) => {
    return Math.max(0, Math.trunc(Number.isFinite(stock) ? stock : 0));
}

const InventoryPage = () => {

    const { user } = useAuth();
    const userRole = user?.role || "Barista";
    const [products, setProducts] = useState<InventoryItem[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const [editingProduct, setEditingProduct] = useState<InventoryItem | null>(null);
    const [isSaving, setIsSaving] = useState(false);
    const [saveError, setSaveError] = useState<string | null>(null);

    const [isClosing, setIsClosing] = useState(false);

    useEffect(() => {
        const fetchInventory = async () => {
            setIsLoading(true);
            try
            {
                const response = await api.get<InventoryItem[]>("/api/inventory");
                setProducts(response.data.map(item => ({
                    ...item,
                    stock: normalizeStock(item.stock)
                })));
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
        }

        fetchInventory();
    }, [])

    const openEditModal = (product: InventoryItem) => {

        setEditingProduct({ ...product, stock: normalizeStock(product.stock) });
        setSaveError(null);
    }

    const closeEditModal = () => {

        setIsClosing(true);
        setTimeout(() => {
            setEditingProduct(null);
            setSaveError(null);
            setIsClosing(false);
        }, 200);
    }

    const handleEditChange = (field: keyof InventoryItem, value: string) => {

            if(!editingProduct) return;

            const current = editingProduct;
            const parsedValue = Number(value);
            const nextValue = field === 'stock'
                ? normalizeStock(parsedValue)
                : field === 'price'
                    ? Number(value)
                    : value;

                setEditingProduct({
                    ...current,
                    [field]: nextValue,
                });
    }

    const handleEditSave = async () => {
        if(!editingProduct) return;
        setIsSaving(true);
        setSaveError(null);

        try{
            const sanitizedProduct = {
                sku: editingProduct.sku,
                name: editingProduct.name,
                price: editingProduct.price,
                stock: normalizeStock(editingProduct.stock)
            };

            await editDrink(sanitizedProduct);
            setProducts(prev => prev.map(p => (p.sku === editingProduct.sku ? sanitizedProduct : p))
        );
        closeEditModal();
        }catch (err){
            console.error("Failed updating product", err)
            setSaveError("The product can't be saved");
        }finally{
            setIsSaving(false);
        }
    }

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
                                        There are no products in the inventory.
                                    </td>
                                </tr>
                            ) : (
                                products.map((product) => (
                                    <tr key={product.sku}>
                                        <td>{product.sku}</td>
                                        <td className="item-name">{product.name}</td>
                                        <td>
                                            <span className="category-badge">General</span>
                                        </td>
                                        <td>${product.price ? product.price.toFixed(2) : "0.00"}</td>
                                        <td className={product.stock < 10 ? 'text-red' : 'text-green'}>
                                            {product.stock}
                                        </td>
                                        {userRole === 'Manager' && (
                                            <td className="text-right">
                                                <button className="action-btn" onClick={() => openEditModal(product)}>
                                                    Edit
                                                </button>
                                            </td>
                                        )}
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                )}
            </div>

            {editingProduct && (
                <div className={`modal-overlay ${isClosing ? 'closing' : ''}`} onClick={closeEditModal}>
                    <div className={`modal-content ${isClosing ? 'closing' : ''}`}   onClick={(e) => e.stopPropagation()}>
                        <h3>Edit product</h3>

                        {saveError && <p className="error-text">{saveError}</p>}

                        <label className="modal-label">
                            Name
                            <input
                                type="text"
                                value={editingProduct.name}
                                onChange={(e) => handleEditChange('name', e.target.value)}
                            />
                        </label>

                        <label className="modal-label">
                            Price
                            <input
                                type="number"
                                step="0.01"
                                value={editingProduct.price}
                                onChange={(e) => handleEditChange('price', e.target.value)}
                            />
                        </label>

                        <label className="modal-label">
                            Stock
                            <input
                                type="number"
                                min="0"
                                step="1"
                                value={editingProduct.stock}
                                onChange={(e) => handleEditChange('stock', e.target.value)}
                            />
                        </label>

                        <div className="modal-actions">
                            <button className="action-btn" onClick={closeEditModal} disabled={isSaving}>
                                Cancel
                            </button>
                            <button className="add-btn" onClick={handleEditSave} disabled={isSaving}>
                                {isSaving ? 'Guardando...' : 'Guardar'}
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default InventoryPage;
