import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { error } from 'selenium-webdriver';
import { SortableDirective, SortDirection, SortEvent } from '../../shared/directives/sortable.directive';
import { AlertService } from '../../shared/services/alert.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { InvitationDto } from '../models/invitationDto';
import { InvitationService } from '../services/invitation.service';

@Component({
  selector: 'app-invitation',
  templateUrl: './invitation.component.html',
  styleUrls: ['./invitation.component.css']
})
export class InvitationComponent implements OnInit {
  invitationAll: InvitationDto[];
  Invitations = ['Undecided', 'Accepted', 'Declined'];
  @ViewChildren(SortableDirective) headers: QueryList<SortableDirective>;

  invitations: InvitationDto[] = [];

  page = 1;
  pageSize = 4;
  sortColumn = '';
  sortDirection: SortDirection = '';
  total = 0;

  constructor(
    private invitationService: InvitationService,
    private errorHandlerService: ErrorHandlerService) { }

  ngOnInit() {
    this.loadInvitations();
  }

  loadInvitations() {
    this.invitationService.getInvitations()
      .subscribe(invitations => {
        this.invitationAll = invitations;
        this.invitations = invitations;
        this.total = invitations.length;
        this.refressInvitations();
      }, error => {
        this.errorHandlerService.handleError(error);
      });
  }

  accept(id: number) {
    this.invitationService.accept(id)
    .subscribe(result => {
      const old = this.invitationAll.find(invitation => invitation.id === result.id);
      const index = this.invitationAll.indexOf(old);
      this.invitationAll[index] = result;
      this.refressInvitations();
    }, error => {
      this.errorHandlerService.handleError(error);
    });
  }

  decline(id: number) {
    this.invitationService.decline(id)
    .subscribe(result => {
      const old = this.invitationAll.find(invitation => invitation.id === result.id);
      const index = this.invitationAll.indexOf(old);
      this.invitationAll[index] = result;
      this.refressInvitations();
    }, error => {
      this.errorHandlerService.handleError(error);
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

    this.refressInvitations();
  }

  refressInvitations() {
    let sortResult = this.sort(this.sortColumn, this.sortDirection);
    sortResult = sortResult.slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
    this.invitations = sortResult;
  }

  sort(column: string, direction: string): InvitationDto[] {
    if (direction === '' || column === '') {
      return this.invitationAll;
    } else {
      return [...this.invitationAll].sort((a, b) => {
        const res = this.compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;
      });
    }
  }

  compare(v1: string | Date, v2: string | Date ) {
    return v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
  }

}
