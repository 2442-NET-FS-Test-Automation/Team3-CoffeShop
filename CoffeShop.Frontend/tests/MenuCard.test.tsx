import { renderToStaticMarkup } from "react-dom/server"
import { describe, expect, it, vi } from "vitest"
import MenuCard from "../src/Components/MenuCards/MenuCard"

describe("MenuCard", () => {
    // TCQ-06: Equivalence partitioning - renders the unavailable state when stock is zero.
    it("renders stock zero and disables adding the product to the cart", () => {
        const onAddToCart = vi.fn()
        const markup = renderToStaticMarkup(
            <MenuCard
                coffee={{
                    productId: 1,
                    name: "American",
                    price: 50,
                    stock: 0,
                }}
                onAddToCart={onAddToCart}
            />,
        )

        expect(markup).toContain("Stock: 0")
        expect(markup).toMatch(/<button[^>]*disabled/)
        expect(onAddToCart).not.toHaveBeenCalled()
    })
})
