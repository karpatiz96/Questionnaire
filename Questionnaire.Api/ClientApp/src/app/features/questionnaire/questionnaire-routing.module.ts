import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
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
  { path: ':id/list', component: QuestionnaireResultAdminComponent }
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QuestionnaireRoutingModule { }
