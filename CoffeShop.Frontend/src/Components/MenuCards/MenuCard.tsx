import type { MenuCardProps } from './menuTypes'

function MenuCard({ coffee, onAddToCart }: MenuCardProps) {
    return (
        <div className="profile-card">
            {coffee.image && (
                <img src={coffee.image} alt={coffee.name} className="menu-card-image" />
            )}

            <h2>Name: {coffee.name}</h2>
            <h2>Price: ${coffee.price}</h2>
            <h2>Stock: {coffee.stock}</h2>

            <button
                onClick={() => onAddToCart(coffee)}
                disabled={coffee.stock <= 0}
            >
                Add to cart
            </button>
        </div>
    )
}

export default MenuCard
