import { useState } from 'react'
import MenuCards from '../Components/MenuCards/MenuCards.tsx'
import type { CoffeeItem } from '../Components/MenuCards/menuTypes'
import americanImg from '../assets/Menu_assets/AMERICAN.png'
import latteImg from '../assets/Menu_assets/LATTE.png'
import capuccinoImg from '../assets/Menu_assets/CAPUCCINO.png'
import taroImg from '../assets/Menu_assets/TARO.png'
import naturalChaiImg from '../assets/Menu_assets/NATURAL_CHAI.png'
import icedLatteImg from '../assets/Menu_assets/ICED_LATTE.png'
import icedAmericanImg from '../assets/Menu_assets/ICED_AMERICAN.png'
import icedTaroImg from '../assets/Menu_assets/ICED_TARO.png'
import icedChaiImg from '../assets/Menu_assets/ICED_CHAI.png'
import './MenuPage.css'

type CartItem = CoffeeItem & {
    quantity: number
}

const coffees: CoffeeItem[] = [
    { productId: 1, name: 'American', price: 50, stock: 5, image: americanImg },
    { productId: 2, name: 'Latte', price: 65, stock: 8, image: latteImg },
    { productId: 3, name: 'Capuccino', price: 60, stock: 4, image: capuccinoImg },
    { productId: 4, name: 'Taro', price: 80, stock: 6, image: taroImg },
    { productId: 5, name: 'Natural Chai', price: 90, stock: 2, image: naturalChaiImg },
    { productId: 6, name: 'Iced Latte', price: 70, stock: 7, image: icedLatteImg },
    { productId: 7, name: 'Iced American', price: 60, stock: 5, image: icedAmericanImg },
    { productId: 8, name: 'Iced Taro', price: 85, stock: 10, image: icedTaroImg },
    { productId: 9, name: 'Iced Chai', price: 95, stock: 3, image: icedChaiImg },
]

function MenuPage() {
    const [cart, setCart] = useState<CartItem[]>([])

    const handleAddToCart = (coffee: CoffeeItem) => {
        setCart((currentCart) => {
            const currentItem = currentCart.find((item) => item.productId === coffee.productId)

            if (currentItem) {
                if (currentItem.quantity >= coffee.stock) {
                    return currentCart
                }

                return currentCart.map((item) =>
                    item.productId === coffee.productId
                        ? { ...item, quantity: item.quantity + 1 }
                        : item
                )
            }

            return [...currentCart, { ...coffee, quantity: 1 }]
        })
    }

    const cartItemCount = cart.reduce((total, item) => total + item.quantity, 0)
    const cartTotal = cart.reduce((total, item) => total + item.quantity * item.price, 0)

    return (
        <section className="menu-page">
            <div className="menu-page-header">
                <div>
                    <p className="menu-page-kicker">Barista counter</p>
                    <h1>Menu</h1>
                </div>

                <div className="cart-summary" aria-label="Cart summary">
                    <span>{cartItemCount} items</span>
                    <strong>${cartTotal}</strong>
                </div>
            </div>

            <MenuCards coffees={coffees} onAddToCart={handleAddToCart} />
        </section>
    )
}

export default MenuPage
