import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationDialogService } from '../../shared/services/confirmationDialog.service';
import { SortableDirective, SortDirection, SortEvent } from '../../shared/directives/sortable.directive';
import { QuestionHeaderDto } from '../models/questionnaires/questionHeaderDto';
import { QuestionnaireDetailsDto } from '../models/questionnaires/questionnaireDetailsDto';
import { QuestionService } from '../services/question.service';
import { QuestionnaireService } from '../services/questionnaire.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { AlertService } from '../../shared/services/alert.service';

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
    randomQuestionOrder: false,
    created: new Date(2020, 1, 1),
    lastEdited: new Date(2020, 1, 1),
    limited: false,
    timeLimit: 0,
    questions: []
  };

  QuestionTypes = ['True or False', 'Multiple Choice', 'Free Text', 'Concrete Text'];

  @ViewChildren(SortableDirective) headers: QueryList<SortableDirective>;

  questions: QuestionHeaderDto[] = [];

  page = 1;
  pageSize = 4;
  sortColumn = '';
  sortDirection: SortDirection = '';
  total = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private questionnaireService: QuestionnaireService,
    private questionService: QuestionService,
    private confirmationDialogService: ConfirmationDialogService,
    private errorHandlerService: ErrorHandlerService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.questionnaireId = params['id'];
      this.loadQuestionnaire(this.questionnaireId);
    });
  }

  loadQuestionnaire(id: number) {
    this.questionnaireService.getDetailsById(id).subscribe(result => {
      this.questionnaire = result;
      this.questions = this.questionnaire.questions;
      this.total = this.questionnaire.questions.length;
      this.refressQuestions();
    }, error => {
      this.errorHandlerService.handleError(error);
    });
  }

  delete(question: QuestionHeaderDto) {
    this.confirmationDialogService.confirm('Delete Question', 'Do you really want to delete the question?').then(result => {
      if (result) {
        this.questionService.delete(question.id)
          .subscribe(result => {
            const index = this.questionnaire.questions.indexOf(question);
            this.questionnaire.questions.splice(index, 1);
            this.refressQuestions();
          }, error => {
            this.errorHandlerService.handleError(error);
            this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
          });
      }
    }).catch(() => {});
  }

  hide(id: number) {
    this.questionnaireService.hide(id).subscribe(() => {
      this.questionnaire.visibleToGroup = false;
    }, error => {
      this.errorHandlerService.handleError(error);
      this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
    });
  }

  show(id: number) {
    this.questionnaireService.show(id).subscribe(() => {
      this.questionnaire.visibleToGroup = true;
    }, error => {
      this.errorHandlerService.handleError(error);
      this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
    });
  }

  copy(id: number) {
    this.questionnaireService.copy(id).subscribe(result => {
      this.router.navigate(['/questionnaire', result.id]);
    }, error => {
      this.errorHandlerService.handleError(error);
      this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
    });
  }

  onSort({column, direction}: SortEvent) {
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    this.sortColumn = column;
    this.sortDirection = direction;

    this.refressQuestions();
  }

  refressQuestions() {
    let sortResult = this.sort(this.sortColumn, this.sortDirection);
    sortResult = sortResult.slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
    this.questions = sortResult;
  }

  sort(column: string, direction: string): QuestionHeaderDto[] {
    if (direction === '' || column === '') {
      return this.questionnaire.questions;
    } else {
      return [...this.questionnaire.questions].sort((a, b) => {
        const res = this.compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;
      });
    }
  }

  compare(v1: string | number, v2: string | number ) {
    return v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
  }
}
