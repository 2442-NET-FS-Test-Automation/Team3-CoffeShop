import { useState, useEffect } from 'react'
import { useLocation } from 'react-router-dom' // <-- Importamos useLocation
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
import { createOrder } from '../api/orders'
import { getInventory } from '../api/inventory'

type SortOrder = 'none' | 'asc' | 'desc'

const productIdBySku: Record<string, number> = {
    'HOT-AME-01': 1,
    'HOT-LAT-02': 2,
    'HOT-CAP-03': 3,
    'HOT-TAR-04': 4,
    'HOT-CHA-05': 5,
    'COL-LAT-06': 6,
    'COL-AME-07': 7,
    'COL-TAR-08': 8,
    'COL-CHA-09': 9,
}

const imageBySku: Record<string, string> = {
    'HOT-AME-01': americanImg,
    'HOT-LAT-02': latteImg,
    'HOT-CAP-03': capuccinoImg,
    'HOT-TAR-04': taroImg,
    'HOT-CHA-05': naturalChaiImg,
    'COL-LAT-06': icedLatteImg,
    'COL-AME-07': icedAmericanImg,
    'COL-TAR-08': icedTaroImg,
    'COL-CHA-09': icedChaiImg,
}

function MenuPage() {
    const location = useLocation() // <-- Hook para leer la ruta actual y su estado
    const [welcomeMessage, setWelcomeMessage] = useState<string | null>(null) // <-- Estado para nuestro mensaje
    
    const [cart, setCart] = useState<CartItem[]>([])
    const [searchTerm, setSearchTerm] = useState<string>('')
    const [sortOrder, setSortOrder] = useState<SortOrder>('none')
    const [isCreatingOrder, setIsCreatingOrder] = useState(false)
    const [orderMessage, setOrderMessage] = useState('')
    const [orderError, setOrderError] = useState('')
    const [coffees, setCoffees] = useState<CoffeeItem[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [loadError, setLoadError] = useState('')

    useEffect(() => {
        if (location.state && location.state.justLoggedIn) {
            setWelcomeMessage('Welcome again!')
            
            window.history.replaceState({}, document.title)

            const timer = setTimeout(() => setWelcomeMessage(null), 4000)
            return () => clearTimeout(timer)
        }
    }, [location])

    useEffect(() => {
        loadMenu()
    }, [])

    const handleToggleSort = () => {
        setSortOrder((currentSortOrder) => {
            if (currentSortOrder === 'none') return 'asc'
            if (currentSortOrder === 'asc') return 'desc'
            return 'none'
        })
    }

    const loadMenu = async () => {
        setIsLoading(true)
        setLoadError('')
        try {
            const inventory  = await getInventory()
            const menuItems = inventory.map((item) => ({
                productId: productIdBySku[item.sku],
                name: item.name,
                price: item.price,
                stock: item.stock,
                image: imageBySku[item.sku]
            }))
            setCoffees(menuItems)
        } catch {
            setLoadError('Could not load menu')
        } finally {
            setIsLoading(false)
        }
    }

    const handleAddToCart = (coffee: CoffeeItem) => {
        setOrderMessage('')
        setOrderError('')
        setCart((currentCart) => {
            const currentItem = currentCart.find((item) => item.productId === coffee.productId)
            if (currentItem) {
                if (currentItem.quantity >= coffee.stock) return currentCart
                return currentCart.map((item) =>
                    item.productId === coffee.productId
                        ? { ...item, quantity: item.quantity + 1 }
                        : item
                )
            }
            return [...currentCart, { ...coffee, quantity: 1 }]
        })
    }

    const handleDecreaseCartItem = (productId: number) => {
        setOrderMessage('')
        setOrderError('')
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

    const handleRemoveCartItem = (productId: number) => {
        setOrderMessage('')
        setOrderError('')
        setCart((currentCart) => currentCart.filter((item) => item.productId !== productId))
    }

    const handleCreateOrder = async () => {
        if (cart.length === 0) {
            setOrderError('Add at least one item before creating an order')
            return
        }

        setIsCreatingOrder(true)
        setOrderMessage('')
        setOrderError('')

        const orderLines = cart.map((item) => ({
            productId: item.productId,
            quantity: item.quantity,
        }))

        try {
            const createdOrder = await createOrder({ lines: orderLines })
            await loadMenu()
            setCart([])
            setOrderMessage(`Order #${createdOrder.orderId} created. Total: $${createdOrder.total.toFixed(2)}`)
        } catch {
            setOrderError('Could not create the order. Check stock and try again.')
        } finally {
            setIsCreatingOrder(false)
        }
    }

    const cartItemCount = cart.reduce((total, item) => total + item.quantity, 0)
    const cartTotal = cart.reduce((total, item) => total + item.quantity * item.price, 0)

    const normalizedSearch = searchTerm.trim().toLowerCase()
    const filteredCoffees = coffees.filter((coffee) =>
        coffee.name.toLowerCase().includes(normalizedSearch)
    )

    const sortedCoffees = [...filteredCoffees].sort((a, b) => {
        if (sortOrder === 'asc') return a.price - b.price
        if (sortOrder === 'desc') return b.price - a.price
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
            
            {/* BURBUJA DE BIENVENIDA FLOTANTE */}
            {welcomeMessage && (
                <div className="floating-success-bubble">
                    {welcomeMessage}
                </div>
            )}

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
                        {isLoading ? (
                            <div className="menu-empty">Loading menu...</div>
                        ) : loadError ? (
                            <div className="menu-empty">{loadError}</div>
                        ) : sortedCoffees.length > 0 ? (
                            <MenuCards coffees={sortedCoffees} onAddToCart={handleAddToCart} />
                        ) : (
                            <div className="menu-empty">No coffees found</div>
                        )}
                    </div>
                </div>

                <CartPanel
                    cart={cart}
                    cartItemCount={cartItemCount}
                    cartTotal={cartTotal}
                    onAddItem={handleAddToCart}
                    onDecreaseItem={handleDecreaseCartItem}
                    onRemoveItem={handleRemoveCartItem}
                    onCreateOrder={handleCreateOrder}
                    isCreatingOrder={isCreatingOrder}
                    orderMessage={orderMessage}
                    orderError={orderError}
                />
            </div>
        </section>
    )
}

export default MenuPage