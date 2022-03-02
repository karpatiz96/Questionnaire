import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionnaireQuestionDto, QuestionType } from '../models/questionnaireQuestionDto';
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

  questions: QuestionnaireQuestionDto [];
  question: QuestionnaireQuestionDto = {
    id: 0,
    questionId: 0,
    description: '',
    questionTitle: '',
    points: 0,
    questionnaireTitle: '',
    type: QuestionType.FreeText,
    answers: []
  };

  questionType = QuestionType;
  questionnaireAnswerForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userQuestionnaireService: UserQuestionnaireService) { }

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
    this.userQuestionnaireService.getQuestionnaireQuestions(this.questionnaireId).subscribe(result => {
      this.questions = result;
      if (this.questions.length > 0) {
        this.question = this.questions[0];

        if (this.questionIndex === this.questions.length - 1) {
          this.final = true;
        }
      }
    }, error => {
      console.log(error);
    });
  }

  get form() {
    return this.questionnaireAnswerForm.controls;
  }

  next() {
    this.questionIndex++;
    if (this.questionIndex < this.questions.length) {
      this.question = this.questions[this.questionIndex];
      this.loading = false;

      if (this.questionIndex === this.questions.length - 1) {
        this.final = true;
      }
    } else {
      this.router.navigate(['/group']);
    }
  }

  submit() {
    this.loading = true;

    const answerDto = new UserQuestionnaireAnswerDto(
      this.question.id,
      this.question.questionId,
      this.questionnaireAnswerForm.controls['answerId'].value,
      this.questionnaireAnswerForm.controls['userAnswer'].value);

    this.userQuestionnaireService.answer(answerDto)
      .subscribe(
        result => {
          this.next();
        },
        error => {
          console.error(error);
          this.loading = false;
      });
  }
}
