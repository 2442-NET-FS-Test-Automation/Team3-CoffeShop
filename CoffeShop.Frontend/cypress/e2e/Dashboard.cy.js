describe ('US 4.1 - Dashboard (E2E)', () => {

    it('TQC-20: Dashboard charts render accuratle when passed mock Sales data', () => {

        cy.intercept('GET', '**/api/Analytics/Analytics', {
            statusCode: 200,
            body: {
                totalOrders: 150,
                totalRevenue: 4850,
                averageTicket: 32.33,
                revenueTrend: 5,
                ticketTrend: 2,
                orderTrend: 1,
                topItems: [
                    { name: "Espresso Americano", unitsSold: 80 },
                    { name: "Latte de Vainilla", unitsSold: 45 },
                    { name: "Capuccino Frio", unitsSold: 25 }
                ],
                salesByHour: [
                    { hour: "08:00", amount: 1200 },
                    { hour: "12:00", amount: 2450 },
                    { hour: "16:00", amount: 1200 }
                ] 
            }
        }).as('getDashboardStats');

        cy.visit("/login");

        cy.get('input[name="username"]').type('Admin');
        cy.get('input[name="password').type('Admin123!');
        cy.get('button[type="submit"]').click();

        cy.url().should('include', '/menu');

        cy.contains('button.nav-link','Dashboard').click();
        cy.url().should('include', '/dashboard');

        cy.wait('@getDashboardStats');

        cy.contains('$4850').should('be.visible');
        cy.contains('150').should('be.visible');
        cy.contains('$32.33').should('be.visible');

        cy.contains('1').should('be.visible');
        cy.contains('Espresso Americano').should('be.visible');
        cy.contains('80').should('be.visible');

        cy.contains('08:00').should('be.visible');
        cy.contains('12:00').should('be.visible');
    });



});