import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { QuestionnaireService } from '../services/questionnaire.service';

@Component({
  selector: 'app-questionnaire-add',
  templateUrl: './questionnaire-add.component.html',
  styleUrls: ['./questionnaire-add.component.css']
})
export class QuestionnaireAddComponent implements OnInit {
  questionnaireForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  groupId: number;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private questionnaireService: QuestionnaireService,
    private errorHandlerService: ErrorHandlerService) { }

  ngOnInit() {
    this.questionnaireForm = this.formBuilder.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      begining: [null , Validators.required],
      finish: [null, Validators.required],
      visibleToGroup: false,
      randomQuestionOrder: false,
      limited: false,
      timeLimit: [1, Validators.min(1)]
    });
    this.route.params.subscribe(params => {
      this.groupId = params['id'];
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
    this.questionnaireService.create(this.questionnaireForm.value, this.groupId)
      .subscribe(
        result => {
          this.router.navigate(['/group', this.groupId]);
        },
        error => {
          this.errorHandlerService.handleError(error);
          this.loading = false;
      });
  }

}
