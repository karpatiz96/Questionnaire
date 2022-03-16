import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvitationComponent } from './invitation/invitation.component';
import { InvitationAddComponent } from './invitation-add/invitation-add.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InvitationRoutingModule } from './invitation-routing.module';
import { SharedModule } from '../shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';



@NgModule({
  declarations: [InvitationComponent, InvitationAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    InvitationRoutingModule,
    SharedModule
  ]
})
export class InvitationModule { }
