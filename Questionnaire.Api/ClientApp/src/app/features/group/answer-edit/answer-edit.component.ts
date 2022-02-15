import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AnswerType } from '../models/answerDto';
import { AnswerService } from '../services/answer.service';
import { QuestionService } from '../services/question.service';

@Component({
  selector: 'app-answer-edit',
  templateUrl: './answer-edit.component.html',
  styleUrls: ['./answer-edit.component.css']
})
export class AnswerEditComponent implements OnInit {
  answerForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  answerId: number;
  eAnswerType = AnswerType;
  keys = Object.keys(AnswerType).filter(key => !isNaN(+key));

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private questionService: QuestionService,
    private answerService: AnswerService) { }

  ngOnInit() {
    this.answerForm = this.formBuilder.group({
      name: ['', Validators.required],
      userAnswer: '',
      value: [0, Validators.required],
      type: [0, Validators.required]
    });

    this.route.params.subscribe(params => {
      this.answerId = params['id'];
      this.loadAnswer(this.answerId);
    });
  }

  private loadAnswer(id: number) {
    this.answerService.getById(id)
    .subscribe(result => {
      this.answerForm.patchValue({
        name: result.name,
        userAnswer: result.userAnswer,
        points: result.value,
        type: result.type
      });
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
    this.answerService.update(this.answerForm.value, this.answerId)
      .subscribe(
        result => {
          this.router.navigate(['/group/answer', this.answerId]);
        },
        error => {
          this.error = error;
          console.error(error);
          this.loading = false;
          this.submitted = false;
        });
  }

}
