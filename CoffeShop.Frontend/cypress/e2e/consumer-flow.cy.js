import { CatalogPage } from "../pages/CatalogPage"

const inventory = [
    { productId: 1, sku: "HOT-AME-01", name: "American", stock: 5, price: 50 },
    { productId: 2, sku: "HOT-LAT-02", name: "Latte", stock: 4, price: 65 },
]

describe("consumer flow", () => {
    beforeEach(() => {
        cy.intercept("GET", "**/api/inventory", {
            statusCode: 200,
            body: inventory,
        }).as("getInventory")

        cy.intercept("POST", "**/api/orders", (req) => {
            expect(req.body).to.deep.equal({
                lines: [{ productId: 1, quantity: 1 }],
            })

            req.reply({
                statusCode: 201,
                body: {
                    orderId: 123,
                    userId: 1,
                    cashierName: "Admin",
                    total: 50,
                    lines: [
                        {
                            orderLineId: 1,
                            productId: 1,
                            productName: "American",
                            quantity: 1,
                            unitPrice: 50,
                            subtotal: 50,
                        },
                    ],
                },
            })
        }).as("createOrder")
    })

    // TCQ-04: Consumer flow - browse menu, add an item, checkout, and verify success.
    it("browse menu, add item, checkout, verify success", () => {
        const catalog = new CatalogPage()

        catalog.open()
        cy.wait("@getInventory")
        cy.contains("Menu")
        cy.contains("American")

        catalog.addItem("American")
        catalog.cartShouldContain("American")
        catalog.cartShouldContain("$50")

        catalog.checkout()
        cy.wait("@createOrder")
        cy.wait("@getInventory")

        catalog.successShouldContain("Order #123 created")
        catalog.successShouldContain("Total: $50.00")
    })
})
