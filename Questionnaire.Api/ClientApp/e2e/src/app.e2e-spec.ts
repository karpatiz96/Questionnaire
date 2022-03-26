import { browser, by, element } from 'protractor';
import { AppPage } from './app.po';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getMainHeading()).toEqual('Hello, world!');
  });

  it('should login', () => {
    page.navigateToLogin().then(() => {
      browser.driver.sleep(5000);
      browser.waitForAngularEnabled().then(() => {
        const email = element(by.id('Input_Email'));
        const password = element(by.id('Input_Password'));
        const logInButton = element(by.buttonText('Log in'));

        email.sendKeys('');
        password.sendKeys('');
        logInButton.click();
      });
    });
  });
});
