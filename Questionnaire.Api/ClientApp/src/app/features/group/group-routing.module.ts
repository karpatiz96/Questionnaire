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
import { QuestionnaireAddComponent } from './questionnaire-add/questionnaire-add.component';
import { QuestionnaireDetailsComponent } from './questionnaire-details/questionnaire-details.component';
import { QuestionAddComponent } from './question-add/question-add.component';
import { QuestionDetailsComponent } from './question-details/question-details.component';
import { QuestionEditComponent } from './question-edit/question-edit.component';
import { QuestionnaireEditComponent } from './questionnaire-edit/questionnaire-edit.component';
import { AnswerAddComponent } from './answer-add/answer-add.component';
import { AnswerDetailsComponent } from './answer-details/answer-details.component';
import { AnswerEditComponent } from './answer-edit/answer-edit.component';

const routes: Routes = [
  { path: '', component: GroupComponent },
  { path: 'add', component: GroupAddComponent },
  { path: 'edit/:id', component: GroupEditComponent },
  { path: 'list', component: GroupListComponent },
  { path: ':id', component: GroupDetailsComponent },
  { path: 'member/:id', component: GroupMemberComponent },
  { path: 'questionnaire/add/:id', component: QuestionnaireAddComponent },
  { path: 'questionnaire/:id', component: QuestionnaireDetailsComponent },
  { path: 'questionnaire/edit/:id', component: QuestionnaireEditComponent },
  { path: 'question/add/:id', component: QuestionAddComponent },
  { path: 'question/:id', component: QuestionDetailsComponent },
  { path: 'question/edit/:id', component: QuestionEditComponent },
  { path: 'answer/add/:id', component: AnswerAddComponent },
  { path: 'answer/:id', component: AnswerDetailsComponent },
  { path: 'answer/edit/:id', component: AnswerEditComponent }
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
