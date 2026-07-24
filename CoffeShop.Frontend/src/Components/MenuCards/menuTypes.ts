export type CoffeeItem = {
    productId: number
    name: string
    price: number
    stock: number
    image?: string
}

export type MenuCardProps = {
    coffee: CoffeeItem
    onAddToCart: (coffee: CoffeeItem) => void
}

export type MenuCardsProps = {
    coffees: CoffeeItem[]
    onAddToCart: (coffee: CoffeeItem) => void
}
