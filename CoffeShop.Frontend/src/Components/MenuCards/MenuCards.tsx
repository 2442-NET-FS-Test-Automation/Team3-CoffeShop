import './MenuCards.css'
import MenuCard from './MenuCard.tsx'
import type { MenuCardsProps } from './menuTypes'

function MenuCards({ coffees, onAddToCart }: MenuCardsProps) {
    return (
        <div className='Menu'>
            {coffees.map((coffee) => (
                <MenuCard
                    key={coffee.productId}
                    coffee={coffee}
                    onAddToCart={onAddToCart}
                />
            ))}
        </div>
    )
}

export default MenuCards
