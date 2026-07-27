import { useState } from 'react'
import MenuCards from '../Components/MenuCards/MenuCards.tsx'
import CartPanel from '../Components/CartPanel/CartPanel.tsx'
import type { CartItem, CoffeeItem } from '../Components/MenuCards/menuTypes'
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

type SortOrder = 'none' | 'asc' | 'desc'

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
    const [searchTerm, setSearchTerm] = useState<string>('')
    const [sortOrder, setSortOrder] = useState<SortOrder>('none')

    const handleToggleSort = () => {
        setSortOrder((currentSortOrder) => {
            if (currentSortOrder === 'none') {
                return 'asc'
            }

            if (currentSortOrder === 'asc') {
                return 'desc'
            }
            return 'none'
        })
    }


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

    // CartPanel calls this when the cashier presses the minus button.
    const handleDecreaseCartItem = (productId: number) => {
        setCart((currentCart) =>
            currentCart
                .map((item) =>
                    item.productId === productId
                        ? { ...item, quantity: item.quantity - 1 }
                        : item
                )
                .filter((item) => item.quantity > 0)
        )
    }

    // CartPanel calls this when the cashier removes a full line from the ticket.
    const handleRemoveCartItem = (productId: number) => {
        setCart((currentCart) => currentCart.filter((item) => item.productId !== productId))
    }

    const handleCreateOrder = () => {
        if (cart.length === 0){
            return;
        } 
        const orderLines = cart.map((item) => ({
        productId: item.productId,
        quantity: item.quantity,
        }))

        const createOrderDto = {
            lines: orderLines,
        }
    }

    // These derived totals are passed into CartPanel so it can stay presentational.
    const cartItemCount = cart.reduce((total, item) => total + item.quantity, 0)
    const cartTotal = cart.reduce((total, item) => total + item.quantity * item.price, 0)

    const normalizedSearch = searchTerm.trim().toLowerCase()
    const filteredCoffees = coffees.filter((coffee) =>
        coffee.name.toLowerCase().includes(normalizedSearch)
    )

    const sortedCoffees = [...filteredCoffees].sort((a, b) => {
        if (sortOrder === 'asc') {
            return a.price - b.price
        }

        if (sortOrder === 'desc') {
            return b.price - a.price
        }

        return 0
    })

    const sortButtonLabel =
        sortOrder === 'asc'
            ? 'Price: low to high'
            : sortOrder === 'desc'
              ? 'Price: high to low'
              : 'Sort by price'

    return (
        <section className="menu-page">
            <div className="menu-workspace">
                <div className="menu-main">
                    <div className="menu-page-header">
                        <div>
                            <p className="menu-page-kicker">Barista counter</p>
                            <h1>Menu</h1>
                        </div>

                        <div className="menu-sort-order">
                            <button
                                type="button"
                                onClick={handleToggleSort}
                                aria-pressed={sortOrder !== 'none'}
                            >
                                {sortButtonLabel}
                            </button>
                        </div>

                        <div className="menu-search-control">
                            <input
                                className="menu-search"
                                type="text"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                                placeholder="Enter the name of your coffee"
                            />
                        </div>
                    </div>

                    <div className="menu-catalog">
                        {sortedCoffees.length > 0 ? (
                            <MenuCards coffees={sortedCoffees} onAddToCart={handleAddToCart} />
                        ) : (
                            <div className="menu-empty">No coffees found</div>
                        )}
                    </div>
                </div>

                {/* CartPanel renders the ticket UI; MenuPage keeps ownership of cart state. */}
                <CartPanel
                    cart={cart}
                    cartItemCount={cartItemCount}
                    cartTotal={cartTotal}
                    onAddItem={handleAddToCart}
                    onDecreaseItem={handleDecreaseCartItem}
                    onRemoveItem={handleRemoveCartItem}
                    onCreateOrder={handleCreateOrder}
                />
            </div>
        </section>
    )
}

export default MenuPage
