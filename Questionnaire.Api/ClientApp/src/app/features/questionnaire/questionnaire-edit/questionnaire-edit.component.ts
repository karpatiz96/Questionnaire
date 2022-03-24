import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../shared/services/alert.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { QuestionnaireService } from '../services/questionnaire.service';

@Component({
  selector: 'app-questionnaire-edit',
  templateUrl: './questionnaire-edit.component.html',
  styleUrls: ['./questionnaire-edit.component.css']
})
export class QuestionnaireEditComponent implements OnInit {
  questionnaireForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  questionnaireId: number;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private questionnaireService: QuestionnaireService,
    private errorHandlerService: ErrorHandlerService,
    private alertService: AlertService) { }

    ngOnInit() {
      this.questionnaireForm = this.formBuilder.group({
        title: ['', Validators.required],
        description: ['', Validators.required],
        begining: ['', Validators.required],
        finish: ['', Validators.required],
        randomQuestionOrder: [false],
        limited: [false],
        timeLimit: [1, Validators.min]
      });
      this.route.params.subscribe(params => {
        this.loadQuestionnaire(params['id']);
        this.questionnaireId = params['id'];
      });
    }

    private loadQuestionnaire(id: number) {
      this.questionnaireService.getById(id)
      .subscribe(result => {
        this.questionnaireForm.patchValue({
          title: result.title,
          description: result.description,
          begining: formatDate(result.begining, 'yyyy-MM-ddThh:mm', 'en_US'),
          finish: formatDate(result.finish, 'yyyy-MM-ddThh:mm', 'en_US'),
          randomQuestionOrder: result.randomQuestionOrder,
          limited: false,
          timeLimit: result.timeLimit
        });
      }, error => {
        this.errorHandlerService.handleError(error);
        this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
      });
    }

    get form() {
      return this.questionnaireForm.controls;
    }

    onSubmit() {
      this.submitted = true;

      if (this.questionnaireForm.invalid) {
        return;
      }

      this.loading = true;
      this.questionnaireService.update(this.questionnaireId, this.questionnaireForm.value)
        .subscribe(
          result => {
            this.router.navigate(['/questionnaire', this.questionnaireId]);
          },
          error => {
            this.errorHandlerService.handleError(error);
            this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
            this.loading = false;
        });
    }

}
