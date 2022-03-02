import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionnaireStartDto } from '../models/questionnaireStartDto';
import { UserQuestionnaireService } from '../services/userQuestionnaireService';

@Component({
  selector: 'app-questionnaire-start',
  templateUrl: './questionnaire-start.component.html',
  styleUrls: ['./questionnaire-start.component.css']
})
export class QuestionnaireStartComponent implements OnInit {
  questionnaireId: number;
  questionnaire: QuestionnaireStartDto = {
    id: 0,
    title: '',
    description: '',
    begining: new Date(2020, 1, 1),
    finish: new Date(2020, 1, 1),
    questions: 0
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userQuestionnaireService: UserQuestionnaireService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.questionnaireId = params['id'];
      this.loadQuestionnaire(this.questionnaireId);
    });
  }

  loadQuestionnaire(id: number) {
    this.userQuestionnaireService.getStartById(id).subscribe(result => {
      this.questionnaire = result;
    });
  }

  begin() {
    this.userQuestionnaireService.start(this.questionnaire.id).subscribe(result => {
      this.router.navigate(['../answer'], { relativeTo: this.route });
    }, error => {
      console.log(error);
    });
  }
}
