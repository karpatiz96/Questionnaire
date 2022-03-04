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
import { QuestionnaireAddComponent } from './questionnaire-add/questionnaire-add.component';
import { QuestionnaireEditComponent } from './questionnaire-edit/questionnaire-edit.component';
import { QuestionnaireDetailsComponent } from './questionnaire-details/questionnaire-details.component';
import { QuestionAddComponent } from './question-add/question-add.component';
import { QuestionEditComponent } from './question-edit/question-edit.component';
import { QuestionDetailsComponent } from './question-details/question-details.component';
import { AnswerAddComponent } from './answer-add/answer-add.component';
import { AnswerEditComponent } from './answer-edit/answer-edit.component';
import { AnswerDetailsComponent } from './answer-details/answer-details.component';
import { SharedModule } from '../shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    QuestionnaireAddComponent,
    QuestionnaireEditComponent,
    QuestionnaireDetailsComponent,
    QuestionAddComponent,
    QuestionEditComponent,
    QuestionDetailsComponent,
    AnswerAddComponent,
    AnswerEditComponent,
    AnswerDetailsComponent,
    QuestionnaireStartComponent,
    QuestionAnswerComponent,
    QuestionnaireAnswerComponent,
    QuestionnaireResultComponent,
    QuestionnaireResultAdminComponent,
    QuestionAnswerEditComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    QuestionnaireRoutingModule,
    SharedModule
  ]
})
export class QuestionnaireModule { }
