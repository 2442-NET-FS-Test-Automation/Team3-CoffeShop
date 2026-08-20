describe('US3.1 - Authentication and Security (E2E)', () => {

    it('TQC-18: Admin UI and Navigation', () => {

        cy.visit("/login");

        cy.get('input[name="username"]').type('Barista2');
        cy.get('input[name="password').type('Barista2!');
        cy.get('button[type="submit"]').click();

        cy.url().should('include', '/menu');

        cy.contains('Dashboard').should('not.exist');
    });

    it('TQC-19: Unauthenticated acces to /dashboard redirects user to /login', () => {

        cy.clearLocalStorage();
        cy.visit('/dashboard');

        cy.url().should('include', '/login');

    });

});