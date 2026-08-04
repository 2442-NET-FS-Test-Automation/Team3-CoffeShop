Cypress.Commands.add("login", (username, password) => {
    cy.request("POST", "http://localhost:5011/auth/login", { username, password })
        .then(({ body }) => {
            window.localStorage.setItem("Access.Token", body.token)
        });
});