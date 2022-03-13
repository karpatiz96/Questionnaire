import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { filter } from 'rxjs/operators';
import { Alert, AlertType } from '../models/alert.model';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  private subject = new Subject<Alert>();

  onAlert(id = 'alert-0') {
    return this.subject.asObservable().pipe(filter(x => x && x.id === id));
  }

  success(message: string, options?: any) {
    this.alert(new Alert({ ...options, type: AlertType.Success, message: message }));
  }

  error(message: string, options?: any) {
    this.alert(new Alert({ ...options, type: AlertType.Error, message: message }));
  }

  alert(alert: Alert) {
    alert.id = alert.id || 'alert-0';
    this.subject.next(alert);
  }

  clear(id = 'alert-0') {
    this.alert(new Alert({id}));
  }
}
