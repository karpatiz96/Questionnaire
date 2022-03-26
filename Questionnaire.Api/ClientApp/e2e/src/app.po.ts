import { browser, by, element } from 'protractor';

export class AppPage {
  navigateTo() {
    return browser.get('/');
  }

  navigateToLogin() {
    return browser.get('http://localhost:5000/authentication/login');
  }

  getMainHeading() {
    return element(by.css('app-root h1')).getText();
  }
}
