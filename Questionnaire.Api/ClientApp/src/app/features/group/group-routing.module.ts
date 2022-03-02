import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { GroupComponent } from './group/group.component';
import { GroupAddComponent } from './group-add/group-add.component';
import { GroupEditComponent } from './group-edit/group-edit.component';
import { GroupListComponent } from './group-list/group-list.component';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { GroupMemberComponent } from './group-member/group-member.component';

const routes: Routes = [
  { path: '', component: GroupComponent },
  { path: 'add', component: GroupAddComponent },
  { path: 'edit/:id', component: GroupEditComponent },
  { path: 'list', component: GroupListComponent },
  { path: ':id', component: GroupDetailsComponent },
  { path: 'member/:id', component: GroupMemberComponent }
];

@NgModule({
    declarations: [],
    imports: [
      CommonModule,
      RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
  })
  export class GroupRoutingModule { }
