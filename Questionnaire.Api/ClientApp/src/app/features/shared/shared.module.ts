import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { ConfirmationDialogService } from './services/confirmationDialog.service';
import { AlertComponent } from './alert/alert.component';



@NgModule({
  declarations: [ConfirmationDialogComponent, AlertComponent],
  imports: [
    CommonModule,
    NgbModule
  ],
  exports: [
    ConfirmationDialogComponent,
    AlertComponent
  ],
  providers: [ConfirmationDialogService],
  entryComponents: [ConfirmationDialogComponent]
})
export class SharedModule { }
