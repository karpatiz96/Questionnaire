import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionnaireStartDto } from '../models/questionnaireStartDto';
import { QuestionnaireService } from '../services/questionnaire.service';

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
    private questionnaireService: QuestionnaireService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.questionnaireId = params['id'];
      this.loadQuestionnaire(this.questionnaireId);
    });
  }

  loadQuestionnaire(id: number) {
    this.questionnaireService.getStartById(id).subscribe(result => {
      this.questionnaire = result;
    });
  }

  begin() {
    this.questionnaireService.start(this.questionnaire.id).subscribe(result => {
      this.router.navigate(['../answer'], { relativeTo: this.route });
    }, error => {
      console.log(error);
    });
  }
}
