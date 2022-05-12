import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupRoutingModule } from './group-routing.module';
import { GroupComponent } from './group/group.component';
import { GroupAddComponent } from './group-add/group-add.component';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { GroupEditComponent } from './group-edit/group-edit.component';
import { GroupListComponent } from './group-list/group-list.component';
import { GroupMemberComponent } from './group-member/group-member.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';



@NgModule({
  declarations: [GroupComponent,
    GroupAddComponent,
    GroupDetailsComponent,
    GroupEditComponent,
    GroupListComponent,
    GroupMemberComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    GroupRoutingModule,
    SharedModule
  ]
})
export class GroupModule { }
