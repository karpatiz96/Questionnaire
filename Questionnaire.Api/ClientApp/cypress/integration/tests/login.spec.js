/// <reference types="cypress" />

describe('Log in test', () => {
    beforeEach(() => {
        cy.visit('https://localhost:5001/authentication/login');
    })

    it('Log in', () => {
        cy.get('input[id=Input_Email]')
            .type('')

        cy.get('input[id=Input_Password]')
            .type('')

        cy.get('button[type=submit]').click();

        cy.get('h1').contains('Hello, world!');
    })
  })
