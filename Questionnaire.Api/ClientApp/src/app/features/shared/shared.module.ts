import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { ConfirmationDialogService } from './services/confirmationDialog.service';



@NgModule({
  declarations: [ConfirmationDialogComponent],
  imports: [
    CommonModule,
    NgbModule
  ],
  exports: [
    ConfirmationDialogComponent
  ],
  providers: [ConfirmationDialogService],
  entryComponents: [ConfirmationDialogComponent]
})
export class SharedModule { }
