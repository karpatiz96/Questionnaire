import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { SortableDirective, SortDirection, SortEvent } from '../../shared/directives/sortable.directive';
import { AlertService } from '../../shared/services/alert.service';
import { ConfirmationDialogService } from '../../shared/services/confirmationDialog.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { GroupMemberDto } from '../models/groupMemberDto';
import { InvitationGroupDto } from '../models/invitationGroupDto';
import { UserGroupDto } from '../models/userGroupDto';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-group-member',
  templateUrl: './group-member.component.html',
  styleUrls: ['./group-member.component.css']
})
export class GroupMemberComponent implements OnInit {
  group: GroupMemberDto = {
    id: 0,
    name: 'Test Group',
    invitations: [],
    users: []
  };
  Invitations = ['Undecided', 'Accepted', 'Declined'];

  @ViewChildren(SortableDirective) headers: QueryList<SortableDirective>;

  users: UserGroupDto[] = [];
  invitations: InvitationGroupDto[] = [];

  userPage = 1;
  userPageSize = 4;
  userSortColumn = '';
  userSortDirection: SortDirection = '';
  userTotal = 0;

  invitationPage = 1;
  invitationPageSize = 4;
  invitationSortColumn = '';
  invitationSortDirection: SortDirection = '';
  invitationTotal = 0;

  constructor(
    private route: ActivatedRoute,
    private groupService: GroupService,
    private confirmationDialogService: ConfirmationDialogService,
    private errorHandlerService: ErrorHandlerService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.loadGroupMembers(params['id']);
    });
  }

  loadGroupMembers(id: number) {
    this.groupService.getMembers(id).subscribe(result => {
      this.group = result;
      this.users = result.users;
      this.invitations = result.invitations;
      this.userTotal = result.users.length;
      this.invitationTotal = result.invitations.length;
    }, error => {
      this.errorHandlerService.handleError(error);
    });
  }

  removeUser(id: number) {
    this.confirmationDialogService
      .confirm('Delete User', 'Do you really want to delete the user?')
        .then(result => {
    }).catch(() => {});
  }

  updateUser(id: number) {
    this.confirmationDialogService
      .confirm('Update User', 'Do you really want to make the user admin in the group?')
        .then(result => {
    }).catch(() => {});
  }

  removeInvitation(id: number) {
    this.confirmationDialogService
      .confirm('Delete User', 'Do you really want to delete the user?')
        .then(result => {
    }).catch(() => {});
  }

  onSortUser({column, direction}: SortEvent) {
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    this.userSortColumn = column;
    this.userSortDirection = direction;

    this.refressUsers();
  }

  refressUsers() {
    let sortResult = this.sortUser(this.userSortColumn, this.userSortDirection);
    sortResult = sortResult.slice((this.userPage - 1) * this.userPageSize, (this.userPage - 1) * this.userPageSize + this.userPageSize);
    this.users = sortResult;
  }

  sortUser(column: string, direction: string): UserGroupDto[] {
    if (direction === '' || column === '') {
      return this.group.users;
    } else {
      return [...this.group.users].sort((a, b) => {
        const res = this.compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;
      });
    }
  }

  onSortInvitation({column, direction}: SortEvent) {
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    this.invitationSortColumn = column;
    this.invitationSortDirection = direction;

    this.refressInvitations();
  }

  refressInvitations() {
    let sortResult = this.sortInvitations(this.invitationSortColumn, this.invitationSortDirection);
    sortResult = sortResult.slice((this.invitationPage - 1) * this.invitationPageSize,
      (this.invitationPage - 1) * this.invitationPageSize + this.invitationPageSize);

    this.invitations = sortResult;
  }

  sortInvitations(column: string, direction: string): InvitationGroupDto[] {
    if (direction === '' || column === '') {
      return this.group.invitations;
    } else {
      return [...this.group.invitations].sort((a, b) => {
        const res = this.compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;
      });
    }
  }

  compare(v1: string | Date, v2: string | Date ) {
    return v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
  }
}
