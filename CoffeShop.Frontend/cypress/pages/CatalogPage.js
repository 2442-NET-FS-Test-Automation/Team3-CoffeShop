// Page Object Model (POM): One class per page of my SPA(or MPA, whatever)
// Selectors and page actions live in this life. A redisign to a page
// gets updates in one place. rather than hunting down dozens of cypress selectors.
// Selenium also has a POM model that we will explore once we get to selenium

const fakeBaristaToken =
    "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJCYXJpc3RhIn0.signature"

export class CatalogPage {
    open() {
        cy.visit("/menu", {
            onBeforeLoad(win) {
                win.localStorage.setItem("Access.Token", fakeBaristaToken)
            },
        })
    }

    addItem(name) {
        cy.contains(".profile-card", `Name: ${name}`)
            .contains("button", "Add to cart")
            .click()
    }

    checkout() {
        cy.contains("button", "Create order").click()
    }

    cartShouldContain(text) {
        cy.get(".cart-panel").should("contain", text)
    }

    successShouldContain(text) {
        cy.get(".cart-status-success").should("contain", text)
    }
}
