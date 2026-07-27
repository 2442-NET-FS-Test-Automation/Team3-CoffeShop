import './CartPanel.css'
import type { CartItem, CoffeeItem } from '../MenuCards/menuTypes'

type CartPanelProps = {
    cart: CartItem[]
    cartItemCount: number
    cartTotal: number
    onAddItem: (coffee: CoffeeItem) => void
    onDecreaseItem: (productId: number) => void
    onRemoveItem: (productId: number) => void
    onCreateOrder: () => void | Promise<void>
    isCreatingOrder: boolean
    orderMessage: string
    orderError: string
}

function CartPanel({
    cart,
    cartItemCount,
    cartTotal,
    onAddItem,
    onDecreaseItem,
    onRemoveItem,
    onCreateOrder,
    isCreatingOrder,
    orderMessage,
    orderError,
}: CartPanelProps) {
    return (
        <aside className="cart-panel" aria-label="Current cart">
            <div className="cart-panel-header">
                <div>
                    <p className="cart-panel-kicker">Current order</p>
                    <h2>Cart</h2>
                </div>
                <span>{cartItemCount} items</span>
            </div>

            {cart.length > 0 ? (
                <div className="cart-items">
                    {cart.map((item) => (
                        <div className="cart-item" key={item.productId}>
                            <div className="cart-item-main">
                                <strong>{item.name}</strong>
                                <span>${item.price} each</span>
                            </div>

                            <div className="cart-item-controls">
                                <button
                                    type="button"
                                    onClick={() => onDecreaseItem(item.productId)}
                                    aria-label={`Decrease ${item.name}`}
                                >
                                    -
                                </button>
                                <span>{item.quantity}</span>
                                <button
                                    type="button"
                                    onClick={() => onAddItem(item)}
                                    disabled={item.quantity >= item.stock}
                                    aria-label={`Increase ${item.name}`}
                                >
                                    +
                                </button>
                            </div>

                            <div className="cart-item-total">
                                <span>${item.price * item.quantity}</span>
                                <button type="button" onClick={() => onRemoveItem(item.productId)}>
                                    Remove
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <div className="cart-empty">No items added</div>
            )}

            <div className="cart-panel-footer">
                <div className="cart-total-row">
                    <span>Total</span>
                    <strong>${cartTotal}</strong>
                </div>
                <button
                    className="cart-checkout"
                    type="button"
                    onClick={onCreateOrder}
                    disabled={cart.length === 0 || isCreatingOrder}
                >
                    {isCreatingOrder ? 'Creating order...' : 'Create order'}
                </button>
                {orderMessage && <p className="cart-status cart-status-success">{orderMessage}</p>}
                {orderError && <p className="cart-status cart-status-error">{orderError}</p>}
            </div>
        </aside>
    )
}

export default CartPanel
