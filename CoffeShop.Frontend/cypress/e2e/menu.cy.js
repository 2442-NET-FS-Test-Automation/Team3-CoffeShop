describe("authenticated user", () => {
    it("authenticated user can open menu", () => {
        cy.login("Admin", "Admin123!")
        cy.visit("/menu")
        cy.contains("Menu")
        cy.contains("American")
    })
})
