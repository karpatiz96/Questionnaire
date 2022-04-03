import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../shared/services/alert.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { AnswerType } from '../models/answers/answerDto';
import { AnswerService } from '../services/answer.service';
import { QuestionService } from '../services/question.service';

@Component({
  selector: 'app-answer-add',
  templateUrl: './answer-add.component.html',
  styleUrls: ['./answer-add.component.css']
})
export class AnswerAddComponent implements OnInit {
  answerForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  questionId: number;
  eAnswerType = AnswerType;
  keys = Object.keys(AnswerType).filter(key => !isNaN(+key));

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private questionService: QuestionService,
    private answerService: AnswerService,
    private errorHandlerService: ErrorHandlerService,
    private alertService: AlertService) {
     }

  ngOnInit() {
    this.answerForm = this.formBuilder.group({
      name: ['', Validators.required],
      userAnswer: '',
      value: [0, [Validators.required, Validators.min(0)]],
      type: [0, Validators.required]
    });

    this.route.params.subscribe(params => {
      this.questionId = params['id'];
    });
  }

  get form() {
    return this.answerForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    if (this.answerForm.invalid) {
      return;
    }

    this.loading = true;
    this.answerService.create(this.answerForm.value, this.questionId)
      .subscribe(
        result => {
          this.router.navigate(['/questionnaire/question', this.questionId]);
        },
        error => {
          this.errorHandlerService.handleError(error);
          this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
          this.loading = false;
          this.submitted = false;
        });
  }

}
