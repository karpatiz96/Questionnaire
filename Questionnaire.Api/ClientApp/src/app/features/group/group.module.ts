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
import { QuestionnaireAddComponent } from './questionnaire-add/questionnaire-add.component';
import { QuestionnaireDetailsComponent } from './questionnaire-details/questionnaire-details.component';
import { QuestionnaireEditComponent } from './questionnaire-edit/questionnaire-edit.component';
import { QuestionAddComponent } from './question-add/question-add.component';
import { QuestionDetailsComponent } from './question-details/question-details.component';
import { QuestionEditComponent } from './question-edit/question-edit.component';
import { AnswerAddComponent } from './answer-add/answer-add.component';
import { AnswerDetailsComponent } from './answer-details/answer-details.component';
import { AnswerEditComponent } from './answer-edit/answer-edit.component';



@NgModule({
  declarations: [GroupComponent,
    GroupAddComponent,
    GroupDetailsComponent,
    GroupEditComponent,
    GroupListComponent,
    GroupMemberComponent,
    QuestionnaireAddComponent,
    QuestionnaireDetailsComponent,
    QuestionnaireEditComponent,
    QuestionAddComponent,
    QuestionDetailsComponent,
    QuestionEditComponent,
    AnswerAddComponent,
    AnswerDetailsComponent,
    AnswerEditComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    GroupRoutingModule
  ]
})
export class GroupModule { }
