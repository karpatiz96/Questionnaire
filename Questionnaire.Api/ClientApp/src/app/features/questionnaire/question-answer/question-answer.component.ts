import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionType } from '../models/questionnaires/questionHeaderDto';
import { UserQuestionnaireAnswerDetailsDto } from '../models/result/userQuestionnaireAnswerDetailsDto';
import { UserQuestionnaireService } from '../services/userQuestionnaireService';

@Component({
  selector: 'app-question-answer',
  templateUrl: './question-answer.component.html',
  styleUrls: ['./question-answer.component.css']
})
export class QuestionAnswerComponent implements OnInit {
  userQuestionnaireAnswerId: number;
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
    answers: []
  };

  questionType = QuestionType;
  QuestionTypes = ['True or False', 'Multiple Choice', 'Free Text', 'Concrete Text'];

  constructor(private route: ActivatedRoute,
    private userQuestionnaireService: UserQuestionnaireService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.userQuestionnaireAnswerId = params['id'];
      this.loadUserQuestionnaireAnswerById(this.userQuestionnaireAnswerId);
    });
  }

  loadUserQuestionnaireAnswerById(id: number) {
    this.userQuestionnaireService.getUserQuestionnaireAnswerById(id)
      .subscribe(result => {
        this.question = result;
    }, error => {
      console.log(error);
    });
  }

}
