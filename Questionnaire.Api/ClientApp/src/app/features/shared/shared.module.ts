import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { ConfirmationDialogService } from './services/confirmationDialog.service';
import { AlertComponent } from './alert/alert.component';
import { SortableDirective } from './directives/sortable.directive';
import { CountDownTimerComponent } from './count-down-timer/count-down-timer.component';



@NgModule({
  declarations: [ConfirmationDialogComponent, AlertComponent, SortableDirective, CountDownTimerComponent],
  imports: [
    CommonModule,
    NgbModule
  ],
  exports: [
    ConfirmationDialogComponent,
    AlertComponent,
    SortableDirective,
    CountDownTimerComponent
  ],
  providers: [ConfirmationDialogService],
  entryComponents: [ConfirmationDialogComponent]
})
export class SharedModule { }
