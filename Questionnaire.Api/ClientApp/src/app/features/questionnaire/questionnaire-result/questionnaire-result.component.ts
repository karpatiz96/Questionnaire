import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionnaireResultDto } from '../models/result/questionnaireResultDto';
import { UserQuestionnaireService } from '../services/userQuestionnaireService';

@Component({
  selector: 'app-questionnaire-result',
  templateUrl: './questionnaire-result.component.html',
  styleUrls: ['./questionnaire-result.component.css']
})
export class QuestionnaireResultComponent implements OnInit {
  userQuestionnaireId: number;
  questionnaire: QuestionnaireResultDto = {
    id: 0,
    title: '',
    description: '',
    begining: new Date(2020, 1, 1),
    finish: new Date(2020, 1, 1),
    maximumPoints: 0,
    points: 0,
    questions: 0,
    answers: []
  };

  QuestionTypes = ['True or False', 'Multiple Choice', 'Free Text', 'Concrete Text'];

  constructor(private route: ActivatedRoute,
    private userQuestionnaireService: UserQuestionnaireService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.userQuestionnaireId = params['id'];
      this.loadQuestionnaireResult(this.userQuestionnaireId);
    });
  }

  loadQuestionnaireResult(id: number) {
    this.userQuestionnaireService.getQuestionnaireResult(id)
      .subscribe(result => {
        this.questionnaire = result;
    }, error => {
      console.log(error);
    });
  }

}
