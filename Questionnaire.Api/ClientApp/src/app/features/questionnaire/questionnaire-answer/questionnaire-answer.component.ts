import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../shared/services/alert.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { QuestionListDto } from '../models/questionListDto';
import { QuestionnaireQuestionListDto } from '../models/questionnaireQuestionListDto';
import { QuestionType } from '../models/questionType';
import { UserQuestionnaireAnswerDto } from '../models/userQuestionnaireAnswerDto';
import { UserQuestionnaireService } from '../services/userQuestionnaireService';

@Component({
  selector: 'app-questionnaire-answer',
  templateUrl: './questionnaire-answer.component.html',
  styleUrls: ['./questionnaire-answer.component.css']
})
export class QuestionnaireAnswerComponent implements OnInit {
  loading = false;
  final = false;
  questionnaireId: number;
  questionIndex = 0;

  questionnaire: QuestionnaireQuestionListDto = {
    id: 0,
    title: '',
    limited: false,
    timeLimit: 5,
    questions: []
  };
  question: QuestionListDto = {
    id: 0,
    description: '',
    title: '',
    points: 0,
    type: QuestionType.FreeText,
    answers: []
  };

  questionType = QuestionType;
  questionnaireAnswerForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userQuestionnaireService: UserQuestionnaireService,
    private errorHandlerService: ErrorHandlerService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.questionnaireAnswerForm = this.formBuilder.group({
      userAnswer: [''],
      answerId: [-1]
    });

    this.route.params.subscribe(params => {
      this.questionnaireId = params['id'];
      this.loadQuestion(this.questionnaireId);
    });
  }

  loadQuestion(questionId: number) {
    this.userQuestionnaireService.getQuestionnaireQuestionList(this.questionnaireId).subscribe(result => {
      this.questionnaire = result;
      if (this.questionnaire.questions.length > 0) {
        this.question = this.questionnaire.questions[0];

        if (this.questionIndex === this.questionnaire.questions.length - 1) {
          this.final = true;
        }
      }
    }, error => {
      this.errorHandlerService.handleError(error);
      this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1'});
    });
  }

  get form() {
    return this.questionnaireAnswerForm.controls;
  }

  next() {
    this.questionIndex++;
    if (this.questionIndex < this.questionnaire.questions.length) {
      this.question = this.questionnaire.questions[this.questionIndex];
      this.loading = false;

      if (this.questionIndex === this.questionnaire.questions.length - 1) {
        this.final = true;
      }
    } else {
      this.router.navigate(['/group']);
    }
  }

  submit() {
    this.loading = true;

    const answerDto = new UserQuestionnaireAnswerDto(
      this.questionnaire.id,
      this.question.id,
      this.questionnaireAnswerForm.controls['answerId'].value,
      this.questionnaireAnswerForm.controls['userAnswer'].value);

    this.userQuestionnaireService.answer(answerDto)
      .subscribe(
        result => {
          this.next();
        },
        error => {
          this.errorHandlerService.handleError(error);
          this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1'});
          this.next();
          this.loading = false;
      });
  }

  timeFinished(notification: string) {
    this.router.navigate(['/group']);
  }
}
