import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionType } from '../models/questionHeaderDto';
import { QuestionService } from '../services/question.service';

@Component({
  selector: 'app-question-add',
  templateUrl: './question-add.component.html',
  styleUrls: ['./question-add.component.css']
})
export class QuestionAddComponent implements OnInit {
  questionForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  questionnaireId: number;
  eQuestionType = QuestionType;
  keys = Object.keys(QuestionType).filter(key => !isNaN(+key));

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private questionService: QuestionService) { }

  ngOnInit() {
    this.questionForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      number: [0, Validators.required],
      value: [0 , Validators.required],
      suggestedTime: [0, Validators.required],
      type: [0, Validators.required]
    });
    this.route.params.subscribe(params => {
      this.questionnaireId = params['id'];
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
    this.questionService.create(this.questionForm.value, this.questionnaireId)
      .subscribe(
        result => {
          this.router.navigate(['/group/questionnaire', this.questionnaireId]);
        },
        error => {
          this.error = error;
          console.error(error);
          this.loading = false;
          this.submitted = false;
        });
  }

}
