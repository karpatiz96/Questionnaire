import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuestionnaireRoutingModule } from './questionnaire-routing.module';
import { QuestionnaireStartComponent } from './questionnaire-start/questionnaire-start.component';
import { QuestionAnswerComponent } from './question-answer/question-answer.component';
import { QuestionnaireAnswerComponent } from './questionnaire-answer/questionnaire-answer.component';
import { ReactiveFormsModule } from '@angular/forms';
import { QuestionnaireResultComponent } from './questionnaire-result/questionnaire-result.component';
import { QuestionnaireResultAdminComponent } from './questionnaire-result-admin/questionnaire-result-admin.component';
import { QuestionAnswerEditComponent } from './question-answer-edit/question-answer-edit.component';


@NgModule({
  declarations: [QuestionnaireStartComponent,
    QuestionAnswerComponent,
    QuestionnaireAnswerComponent,
    QuestionnaireResultComponent,
    QuestionnaireResultAdminComponent,
    QuestionAnswerEditComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    QuestionnaireRoutingModule
  ]
})
export class QuestionnaireModule { }
