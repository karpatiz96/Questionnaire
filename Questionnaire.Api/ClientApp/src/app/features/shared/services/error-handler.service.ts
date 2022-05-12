import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AlertService } from './alert.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  public errorMessage = '';

  constructor(private router: Router,
    private alertService: AlertService) { }

    public handleError(error: HttpErrorResponse) {
      if (error.status === 403) {
        this.handleError403(error);
      } else if (error.status === 404) {
        this.handleError404(error);
      } else {
        this.handleErrorOther(error);
      }
    }

    private handleError403(error: HttpErrorResponse) {
      this.errorMessage = error.error.message ? error.error.message : '';
      this.alertService.error(this.errorMessage, {keepAlertAfterRouteChange: true, id: 'alert-0' });
      this.router.navigate(['/forbidden']);
    }

    private handleError404(error: HttpErrorResponse) {
      this.errorMessage = error.error.message ? error.error.message : '';
      this.alertService.error(this.errorMessage, {keepAlertAfterRouteChange: true, id: 'alert-0' });
      this.router.navigate(['/not-found']);
    }

    private handleErrorOther(error: HttpErrorResponse) {
      this.errorMessage = error.error.message ? error.error.message : '';
    }
}
