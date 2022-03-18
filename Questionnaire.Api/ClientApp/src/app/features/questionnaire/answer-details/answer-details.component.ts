import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionType } from '../../questionnaire/models/questionnaires/questionHeaderDto';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { AnswerDetailsDto } from '../models/answers/answerDetailsDto';
import { AnswerType } from '../models/answers/answerDto';
import { AnswerService } from '../services/answer.service';

@Component({
  selector: 'app-answer-details',
  templateUrl: './answer-details.component.html',
  styleUrls: ['./answer-details.component.css']
})
export class AnswerDetailsComponent implements OnInit {
  answerId: number;
  answer: AnswerDetailsDto = {
    id: 0,
    questionType: QuestionType.ConcreteText,
    name: '',
    type: AnswerType.Correct,
    userAnswer: '',
    value: 0,
    visibleToGroup: false
  };

  AnswerTypes = ['Correct', 'False'];

  constructor(
    private route: ActivatedRoute,
    private answerService: AnswerService,
    private errorHandlerSerive: ErrorHandlerService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.answerId = params['id'];
      this.loadAnswer(this.answerId);
    });
  }

  loadAnswer(id: number) {
    this.answerService.getById(id).subscribe(result => {
      this.answer = result;
    }, error => {
      this.errorHandlerSerive.handleError(error);
    });
  }

}
