import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { SortableDirective, SortDirection, SortEvent } from '../../shared/directives/sortable.directive';
import { QuestionnaireResultHeaderDto, QuestionnaireResultListDto } from '../models/result/questionnaireResultListDto';
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

  @ViewChildren(SortableDirective) headers: QueryList<SortableDirective>;

  results: QuestionnaireResultHeaderDto[] = [];

  page = 1;
  pageSize = 4;
  sortColumn = '';
  sortDirection: SortDirection = '';
  total = 0;

  constructor(private route: ActivatedRoute,
    private userQuestionnaireService: UserQuestionnaireService,
    private errorHandlerService: ErrorHandlerService) { }

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
        this.results = this.questionnaire.results;
        this.total = this.questionnaire.results.length;
        this.refressResults();
    }, error => {
      this.errorHandlerService.handleError(error);
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

    this.refressResults();
  }

  refressResults() {
    let sortResult = this.sort(this.sortColumn, this.sortDirection);
    sortResult = sortResult.slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
    this.results = sortResult;
  }

  sort(column: string, direction: string): QuestionnaireResultHeaderDto[] {
    if (direction === '' || column === '') {
      return this.questionnaire.results;
    } else {
      return [...this.questionnaire.results].sort((a, b) => {
        const res = this.compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;
      });
    }
  }

  compare(v1: string | number, v2: string | number ) {
    return v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
  }
}
