import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AnswerDetailsDto } from '../models/answerDetailsDto';
import { AnswerType } from '../models/answerDto';
import { QuestionType } from '../models/questionHeaderDto';
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
    value: 0
  };

  AnswerTypes = ['Correct', 'False'];

  constructor(
    private route: ActivatedRoute,
    private answerService: AnswerService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.answerId = params['id'];
      this.loadAnswer(this.answerId);
    });
  }

  loadAnswer(id: number) {
    this.answerService.getById(id).subscribe(result => {
      this.answer = result;
    });
  }

}
