import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionType } from '../../questionnaire/models/questionnaires/questionHeaderDto';
import { AlertService } from '../../shared/services/alert.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { QuestionService } from '../services/question.service';

@Component({
  selector: 'app-question-edit',
  templateUrl: './question-edit.component.html',
  styleUrls: ['./question-edit.component.css']
})
export class QuestionEditComponent implements OnInit {
  questionForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  questionId: number;
  eQuestionType = QuestionType;
  keys = Object.keys(QuestionType).filter(key => !isNaN(+key));

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private questionService: QuestionService,
    private errorHandlerService: ErrorHandlerService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.questionForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      number: [1, [Validators.required, Validators.min(1)]],
      value: [0 , [Validators.required, Validators.min(0)]],
      suggestedTime: [0, [Validators.required, Validators.min(0)]],
      type: [0, Validators.required]
    });
    this.route.params.subscribe(params => {
      this.questionId = params['id'];
      this.loadQuestion(this.questionId);
    });
  }

  private loadQuestion(id: number) {
    this.questionService.getById(id)
    .subscribe(result => {
      this.questionForm.patchValue({
        name: result.name,
        description: result.description,
        number: result.number,
        value: result.value,
        suggestedTime: result.suggestedTime,
        type: result.type
      });
    }, error => {
      this.errorHandlerService.handleError(error);
    });
  }

  get form() {
    return this.questionForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    if (this.questionForm.invalid) {
      return;
    }

    this.loading = true;
    this.questionService.update(this.questionForm.value, this.questionId).subscribe(
        result => {
          this.router.navigate(['/questionnaire/question', this.questionId]);
        },
        error => {
          this.errorHandlerService.handleError(error);
          this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1'});
          this.loading = false;
          this.submitted = false;
        });
  }

}
