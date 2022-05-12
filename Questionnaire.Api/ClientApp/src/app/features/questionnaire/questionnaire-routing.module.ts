import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AnswerAddComponent } from './answer-add/answer-add.component';
import { AnswerDetailsComponent } from './answer-details/answer-details.component';
import { AnswerEditComponent } from './answer-edit/answer-edit.component';
import { QuestionAddComponent } from './question-add/question-add.component';
import { QuestionAnswerEditComponent } from './question-answer-edit/question-answer-edit.component';
import { QuestionAnswerComponent } from './question-answer/question-answer.component';
import { QuestionDetailsComponent } from './question-details/question-details.component';
import { QuestionEditComponent } from './question-edit/question-edit.component';
import { QuestionnaireAddComponent } from './questionnaire-add/questionnaire-add.component';
import { QuestionnaireAnswerComponent } from './questionnaire-answer/questionnaire-answer.component';
import { QuestionnaireDetailsComponent } from './questionnaire-details/questionnaire-details.component';
import { QuestionnaireEditComponent } from './questionnaire-edit/questionnaire-edit.component';
import { QuestionnaireResultAdminComponent } from './questionnaire-result-admin/questionnaire-result-admin.component';
import { QuestionnaireResultComponent } from './questionnaire-result/questionnaire-result.component';
import { QuestionnaireStartComponent } from './questionnaire-start/questionnaire-start.component';


const routes: Routes = [
  { path: 'add/:id', component: QuestionnaireAddComponent },
  { path: ':id', component: QuestionnaireDetailsComponent },
  { path: 'edit/:id', component: QuestionnaireEditComponent },
  { path: 'question/add/:id', component: QuestionAddComponent },
  { path: 'question/:id', component: QuestionDetailsComponent },
  { path: 'question/edit/:id', component: QuestionEditComponent },
  { path: 'answer/add/:id', component: AnswerAddComponent },
  { path: 'answer/:id', component: AnswerDetailsComponent },
  { path: 'answer/edit/:id', component: AnswerEditComponent },
  { path: ':id/start', component: QuestionnaireStartComponent },
  { path: ':id/answer', component: QuestionnaireAnswerComponent },
  { path: 'result/:id', component: QuestionnaireResultComponent },
  { path: 'result/question/:id', component: QuestionAnswerComponent },
  { path: 'result/question/:id/edit', component: QuestionAnswerEditComponent },
  { path: ':id/list', component: QuestionnaireResultAdminComponent }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QuestionnaireRoutingModule { }
