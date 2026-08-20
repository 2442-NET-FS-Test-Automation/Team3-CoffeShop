describe("authenticated user", () => {
    const fakeManagerToken =
        "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJNYW5hZ2VyIn0.signature"

    beforeEach(() => {
        cy.intercept("GET", "**/api/inventory", {
            statusCode: 200,
            body: [
                { productId: 1, sku: "HOT-AME-01", name: "American", stock: 5, price: 50 },
            ],
        }).as("getInventory")
    })

    it("authenticated user can open menu", () => {
        cy.visit("/menu", {
            onBeforeLoad(win) {
                win.localStorage.setItem("Access.Token", fakeManagerToken)
            },
        })

        cy.wait("@getInventory")
        cy.contains("Menu")
        cy.contains("American")
    })
})
