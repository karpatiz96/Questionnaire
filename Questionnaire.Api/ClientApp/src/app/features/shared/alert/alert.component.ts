import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Alert, AlertType } from '../models/alert.model';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent implements OnInit, OnDestroy {
  @Input() id = 'alert-0';

  alerts: Alert[] = [];
  private alertSubscription: Subscription;
  private routeSubscription: Subscription;

  constructor(private router: Router,
    private alertService: AlertService) { }

  ngOnInit() {
    this.alertSubscription = this.alertService.onAlert(this.id)
      .subscribe(alert => {
        if (!alert.message) {
          this.alerts = this.alerts.filter(x => x.keepAlertAfterRouteChange);
          this.alerts.forEach(x => x.keepAlertAfterRouteChange = false);
          return;
        }

        this.alerts.push(alert);
      });

      this.routeSubscription = this.router.events.subscribe(event => {
        if (event instanceof NavigationStart) {
          this.alertService.clear(this.id);
        }
      });
  }

  ngOnDestroy(): void {
    this.alertSubscription.unsubscribe();
    this.routeSubscription.unsubscribe();
  }

  removeAlert(alert: Alert) {
    if (!this.alerts.includes(alert)) { return; }

    this.alerts.splice(this.alerts.indexOf(alert) , 1);
  }

  getClass(alert: Alert) {
    if (!alert) {
      return;
    }

    const alertType = {
      [AlertType.Success]: 'alert alert-success alert-dismissable',
      [AlertType.Error]: 'alert alert-danger alert-dismissable'
    };

    return alertType[alert.type];
  }
}
