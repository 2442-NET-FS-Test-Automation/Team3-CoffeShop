describe('login smoke test', () => {
    it('redirects home to login', () => {
        cy.visit("/")
        cy.url().should("include","/login")
        cy.contains("Login")
        cy.contains("button", "Submit")
    })
})

describe('login', () => {
    beforeEach(() =>{
        cy.visit("/login")

    })
    it('invalid login', ()=> {
        cy.intercept('POST', '**/auth/login', {
            statusCode:401,
            body: {}
        }).as('loginRequest');
        cy.get("#username").type("baduser")
        cy.get("#password").type("badpass")
        cy.contains("button", "Submit").click()
        cy.wait("@loginRequest")
        cy.contains("User or password are not valid.")
        cy.url().should("include", "/login")
    })
    it('valid login', () => {
        const fakeToken = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJNYW5hZ2VyIn0.signature"

        cy.intercept('POST', '**/auth/login', {
            statusCode: 200,
            body: { token: fakeToken }
        }).as('loginRequest')

        cy.intercept('GET', '**/api/inventory', {
            statusCode: 200,
            body: [
                { productId: 1, sku: 'HOT-AME-01', name: 'American', stock: 5, price: 50 }
            ]
        }).as('getInventory')

        cy.get("#username").type("Admin")
        cy.get("#password").type("anything")
        cy.contains("button", "Submit").click()
        cy.wait("@loginRequest")
        cy.wait("@getInventory")
        cy.url().should("include", "/menu")
        cy.contains("Menu")
        cy.contains("American")
    })
});
