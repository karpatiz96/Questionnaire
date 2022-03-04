import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ConfirmationDialogService } from '../../shared/services/confirmationDialog.service';
import { QuestionnaireDetailsDto } from '../models/questionnaires/questionnaireDetailsDto';
import { QuestionnaireService } from '../services/questionnaire.service';

@Component({
  selector: 'app-questionnaire-details',
  templateUrl: './questionnaire-details.component.html',
  styleUrls: ['./questionnaire-details.component.css']
})
export class QuestionnaireDetailsComponent implements OnInit {
  questionnaireId: number;
  questionnaire: QuestionnaireDetailsDto = {
    id: 0,
    title: '',
    description: '',
    begining: new Date(2020, 1, 1),
    finish: new Date(2020, 1, 1),
    visibleToGroup: false,
    created: new Date(2020, 1, 1),
    lastEdited: new Date(2020, 1, 1),
    questions: []
  };

  QuestionTypes = ['True or False', 'Multiple Choice', 'Free Text', 'Concrete Text'];

  constructor(
    private route: ActivatedRoute,
    private questionnaireService: QuestionnaireService,
    private confirmationDialogService: ConfirmationDialogService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.questionnaireId = params['id'];
      this.loadQuestionnaire(this.questionnaireId);
    });
  }

  loadQuestionnaire(id: number) {
    this.questionnaireService.getById(id).subscribe(result => {
      this.questionnaire = result;
    }, error => {
      console.log(error);
    });
  }

  delete(questionId: number) {
    this.confirmationDialogService.confirm('Delete Answer', 'Do you really want to delete the answer?').then(result => {
      console.log(result);
    }).catch(() => {});

  }
}