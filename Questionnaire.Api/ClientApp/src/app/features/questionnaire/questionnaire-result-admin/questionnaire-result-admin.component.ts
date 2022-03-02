import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionnaireResultListDto } from '../models/result/questionnaireResultListDto';
import { UserQuestionnaireService } from '../services/userQuestionnaireService';

@Component({
  selector: 'app-questionnaire-result-admin',
  templateUrl: './questionnaire-result-admin.component.html',
  styleUrls: ['./questionnaire-result-admin.component.css']
})
export class QuestionnaireResultAdminComponent implements OnInit {
  questionnaireId: number;
  questionnaire: QuestionnaireResultListDto = {
    id: 0,
    title: '',
    description: '',
    begining: new Date(2020, 1, 1),
    finish: new Date(2020, 1, 1),
    members: 0,
    solved: 0,
    questions: 0,
    results: []
  };

  constructor(private route: ActivatedRoute,
    private userQuestionnaireService: UserQuestionnaireService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.questionnaireId = params['id'];
      this.loadQuestionnaireResult(this.questionnaireId);
    });
  }

  loadQuestionnaireResult(id: number) {
    this.userQuestionnaireService.getQuestionnaireResultAdmin(id)
      .subscribe(result => {
        this.questionnaire = result;
    }, error => {
      console.log(error);
    });
  }
}
