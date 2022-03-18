import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { GroupDetailsDto } from '../models/groupDetailsDto';
import { QuestionnaireHeaderDto } from '../models/questionnaireHeaderDto';
import { GroupService } from '../services/group.service';
import { QuestionnaireService } from '../services/questionnaire.service';

@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.css']
})
export class GroupDetailsComponent implements OnInit {
  questionnaireForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  group: GroupDetailsDto = {
    id: 0,
    description: 'Test',
    name: 'Test Group',
    created: new Date(2021, 1, 1),
    lastPost: new Date(2021, 1, 1),
    groupRole: 'User',
    members: 1,
    questionnaires: []
  };

  questionnaires: QuestionnaireHeaderDto[] = [];

  page = 1;
  pageSize = 4;
  total = 0;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private groupService: GroupService,
    private questionnaireService: QuestionnaireService,
    private errorHandlerSerive: ErrorHandlerService) { }

  ngOnInit() {
    this.questionnaireForm = this.formBuilder.group({
      to: [null],
      from: [null],
      visible: [true]
    });

    this.route.params.subscribe(params => {
      this.loadGroup(params['id']);
    });
  }

  loadGroup(id: number) {
    this.groupService.getById(id).subscribe(result => {
      this.group = result;
      this.total = result.questionnaires.length;
      this.refressQuestionnaires();
    }, error => {
      this.errorHandlerSerive.handleError(error);
    });
  }

  get form() {
    return this.questionnaireForm.controls;
  }

  searchQuestionnaires() {
    this.submitted = true;

    if (this.questionnaireForm.invalid) {
      return;
    }

    this.loading = true;
    this.questionnaireService.getList(this.group.id, this.questionnaireForm.value).subscribe(result => {
      this.submitted = false;
      this.loading = false;
      this.group.questionnaires = result;
      this.total = result.length;
      this.refressQuestionnaires();
    }, error => {
      this.loading = false;
      this.errorHandlerSerive.handleError(error);
    });
  }

  refressQuestionnaires() {
    this.questionnaires = this.group.questionnaires.slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
  }
}
