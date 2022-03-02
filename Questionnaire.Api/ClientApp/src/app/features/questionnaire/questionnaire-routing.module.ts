import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuestionAnswerEditComponent } from './question-answer-edit/question-answer-edit.component';
import { QuestionAnswerComponent } from './question-answer/question-answer.component';
import { QuestionnaireAnswerComponent } from './questionnaire-answer/questionnaire-answer.component';
import { QuestionnaireResultAdminComponent } from './questionnaire-result-admin/questionnaire-result-admin.component';
import { QuestionnaireResultComponent } from './questionnaire-result/questionnaire-result.component';
import { QuestionnaireStartComponent } from './questionnaire-start/questionnaire-start.component';


const routes: Routes = [
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
