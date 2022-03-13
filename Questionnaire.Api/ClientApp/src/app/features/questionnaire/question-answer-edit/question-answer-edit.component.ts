import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionType } from '../models/questionnaireQuestionDto';
import { UserQuestionnaireAnswerDetailsDto } from '../models/result/userQuestionnaireAnswerDetailsDto';
import { UserQuestionnaireService } from '../services/userQuestionnaireService';

@Component({
  selector: 'app-question-answer-edit',
  templateUrl: './question-answer-edit.component.html',
  styleUrls: ['./question-answer-edit.component.css']
})
export class QuestionAnswerEditComponent implements OnInit {
  userQuestionnaireAnswerForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  userQuestionnaireAnswerId: number;
  questionType = QuestionType;
  QuestionTypes = ['True or False', 'Multiple Choice', 'Free Text', 'Concrete Text'];
  question: UserQuestionnaireAnswerDetailsDto = {
    id: 0,
    questionnaireTitle: '',
    questionTitle: '',
    description: '',
    type: QuestionType.ConcreteText,
    maximumPoints: 0,
    points: 0,
    userAnswer: '',
    answerId: 0,
    answers: [],
    role: 'User'
  };

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private userQuestionnaireService: UserQuestionnaireService) { }

  ngOnInit() {
    this.userQuestionnaireAnswerForm = this.formBuilder.group({
      points: [0]
    });

    this.route.params.subscribe(params => {
      this.userQuestionnaireAnswerId = params['id'];
      this.loadUserQuestionnaireAnswerById(this.userQuestionnaireAnswerId);
    });
  }

  loadUserQuestionnaireAnswerById(id: number) {
    this.userQuestionnaireService.getUserQuestionnaireAnswerById(id)
      .subscribe(result => {
        this.question = result;
        this.form['points'].setValidators([Validators.required, Validators.min(0), Validators.max(result.maximumPoints)]);
    }, error => {
      console.log(error);
    });
  }

  get form() {
    return this.userQuestionnaireAnswerForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    if (this.userQuestionnaireAnswerForm.invalid) {
      return;
    }

    this.loading = true;
    this.userQuestionnaireService.evaluateQuestion(this.userQuestionnaireAnswerId, this.userQuestionnaireAnswerForm.value)
      .subscribe(
        result => {
          this.router.navigate(['/questionnaire/result/question', this.userQuestionnaireAnswerId]);
        },
        error => {
          this.error = error;
          console.error(error);
          this.loading = false;
      });
  }
}
