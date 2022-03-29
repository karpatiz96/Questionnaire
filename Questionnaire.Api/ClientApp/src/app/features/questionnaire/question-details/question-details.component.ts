import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionType } from '../../questionnaire/models/questionnaires/questionHeaderDto';
import { AlertService } from '../../shared/services/alert.service';
import { ConfirmationDialogService } from '../../shared/services/confirmationDialog.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { AnswerHeaderDto } from '../models/questions/answerHeaderDto';
import { QuestionDetailsDto } from '../models/questions/questionDetailsDto';
import { AnswerService } from '../services/answer.service';
import { QuestionService } from '../services/question.service';

@Component({
  selector: 'app-question-details',
  templateUrl: './question-details.component.html',
  styleUrls: ['./question-details.component.css']
})
export class QuestionDetailsComponent implements OnInit {
  questionId: number;
  question: QuestionDetailsDto = {
    id: 0,
    questionnaireId: 0,
    name: '',
    description: '',
    number: 0,
    suggestedTime: 0,
    type: QuestionType.ConcreteText,
    value: 0,
    answers: [],
    visibleToGroup: false
  };

  QuestionTypes = ['True or False', 'Multiple Choice', 'Free Text', 'Concrete Text'];
  AnswerTypes = ['Correct', 'False'];

  constructor(
    private route: ActivatedRoute,
    private questionService: QuestionService,
    private answerService: AnswerService,
    private confirmationDialogService: ConfirmationDialogService,
    private errorHandlerSerivce: ErrorHandlerService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.questionId = params['id'];
      this.loadQuestion(this.questionId);
    });
  }

  loadQuestion(id: number) {
    this.questionService.getDetailsById(id).subscribe(result => {
      this.question = result;
    }, error => {
      this.errorHandlerSerivce.handleError(error);
    });
  }

  delete(answer: AnswerHeaderDto) {
    this.confirmationDialogService.confirm('Delete Answer', 'Do you really want to delete the answer?').then(result => {
      if (result) {
        this.answerService.delete(answer.id)
        .subscribe(() => {
          const index = this.question.answers.indexOf(answer);
          this.question.answers.splice(index, 1);
        }, error => {
          this.errorHandlerSerivce.handleError(error);
          this.alertService.error(this.errorHandlerSerivce.errorMessage, { id: 'alert-1' });
        });
      }
    }).catch(() => {});

  }

}
