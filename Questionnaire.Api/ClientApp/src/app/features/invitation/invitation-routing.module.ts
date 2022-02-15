import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { InvitationComponent } from './invitation/invitation.component';
import { InvitationAddComponent } from './invitation-add/invitation-add.component';

const routes: Routes = [
  { path: '', component: InvitationComponent },
  { path: 'add', component: InvitationAddComponent }
];

@NgModule({
    declarations: [],
    imports: [
      CommonModule,
      RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
  })
  export class InvitationRoutingModule { }
