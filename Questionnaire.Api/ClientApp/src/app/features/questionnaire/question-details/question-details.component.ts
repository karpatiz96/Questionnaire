import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionType } from '../../questionnaire/models/questionnaires/questionHeaderDto';
import { ConfirmationDialogService } from '../../shared/services/confirmationDialog.service';
import { QuestionDetailsDto } from '../models/questions/questionDetailsDto';
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
    answers: []
  };

  QuestionTypes = ['True or False', 'Multiple Choice', 'Free Text', 'Concrete Text'];
  AnswerTypes = ['Correct', 'False'];

  constructor(
    private route: ActivatedRoute,
    private questionService: QuestionService,
    private confirmationDialogService: ConfirmationDialogService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.questionId = params['id'];
      this.loadQuestion(this.questionId);
    });
  }

  loadQuestion(id: number) {
    this.questionService.getById(id).subscribe(result => {
      this.question = result;
    });
  }

  delete(answerId: number) {
    this.confirmationDialogService.confirm('Delete Answer', 'Do you really want to delete the answer?').then(result => {
      console.log(result);
    }).catch(() => {});

  }

}
