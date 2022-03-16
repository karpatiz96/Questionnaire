import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { SortableDirective, SortDirection, SortEvent } from '../../shared/directives/sortable.directive';
import { QuestionType } from '../models/questionnaireQuestionDto';
import { QuestionnaireResultDto, UserQuestionAnswerHeaderDto } from '../models/result/questionnaireResultDto';
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
    userName: '',
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

  @ViewChildren(SortableDirective) headers: QueryList<SortableDirective>;

  answers: UserQuestionAnswerHeaderDto[] = [];

  page = 1;
  pageSize = 4;
  sortColumn = '';
  sortDirection: SortDirection = '';
  total = 0;

  /*private _answers = new BehaviorSubject<UserQuestionAnswerHeaderDto[]>([]);
  private _page = 1;
  private _pageSize = 4;
  private _sortColumn = '';
  private _sortDirection: SortDirection = '';
  private _total = new BehaviorSubject<Number>(0);

  get answers() { return this._answers.asObservable(); }
  get page() { return this._page; }
  set page(page: number) {
    this._page = page;
    this.onSort({column: this.sortColumn, direction: this.sortDirection});
  }
  get pageSize() { return this._pageSize; }
  set pageSize(pageSize: number) {
    this._pageSize = pageSize;
    this.onSort({column: this.sortColumn, direction: this._sortDirection});
  }
  get sortColumn() { return this._sortColumn; }
  set sortColumn(column: string) { this._sortColumn = column; }
  get sortDirection() { return this._sortDirection; }
  set sortDirection(direction: SortDirection) { this._sortDirection = direction; }
  get total() { return this._total.asObservable(); }*/

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
        this.answers = result.answers;
        this.total = result.answers.length;
        this.refressAnswers();
    }, error => {
      console.log(error);
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

    this.refressAnswers();
  }

  refressAnswers() {
    let result = this.sort(this.sortColumn, this.sortDirection);
    result = result.slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
    this.answers = result;
  }

  sort(column: string, direction: string): UserQuestionAnswerHeaderDto[] {
    if (direction === '' || column === '') {
      return this.questionnaire.answers;
    } else {
      return [...this.questionnaire.answers].sort((a, b) => {
        const res = this.compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;
      });
    }
  }

  compare(v1: string | QuestionType, v2: string | QuestionType ) {
    return v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
  }

}

