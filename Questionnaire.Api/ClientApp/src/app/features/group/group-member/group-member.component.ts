import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SortableDirective, SortDirection, SortEvent } from '../../shared/directives/sortable.directive';
import { AlertService } from '../../shared/services/alert.service';
import { ConfirmationDialogService } from '../../shared/services/confirmationDialog.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { GroupMemberDto } from '../models/groupMemberDto';
import { InvitationGroupDto } from '../models/invitationGroupDto';
import { UserGroupDto } from '../models/userGroupDto';
import { GroupService } from '../services/group.service';
import { InvitationService } from '../services/invitation.service';
import { UserGroupService } from '../services/user-group.service';

@Component({
  selector: 'app-group-member',
  templateUrl: './group-member.component.html',
  styleUrls: ['./group-member.component.css']
})
export class GroupMemberComponent implements OnInit {
  groupId: number;
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
    private userGroupService: UserGroupService,
    private invitationService: InvitationService,
    private confirmationDialogService: ConfirmationDialogService,
    private errorHandlerService: ErrorHandlerService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.groupId = params['id'];
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
          if (result) {
            this.userGroupService.delete(id).subscribe(() => {
              const user = this.group.users.find(u => u.id === id);
              const index = this.group.users.indexOf(user);
              this.group.users.splice(index, 1);
              this.refressUsers();
            }, error => {
                this.errorHandlerService.handleError(error);
                this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
            });
          }
    }).catch(() => {});
  }

  makeAdmin(id: number) {
    this.confirmationDialogService
      .confirm('Update User', 'Do you really want to make the user admin in the group?')
        .then(result => {
          if (result) {
              this.userGroupService.makeAdmin(id).subscribe(() => {
              this.loadGroupMembers(this.groupId);
            }, error => {
                this.errorHandlerService.handleError(error);
                this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
            });
          }
        }).catch(() => {});
  }

  makeUser(id: number) {
    this.confirmationDialogService
      .confirm('Update User', 'Do you really want to revoke admin?')
        .then(result => {
          if (result) {
              this.userGroupService.makeUser(id).subscribe(() => {
              this.loadGroupMembers(this.groupId);
            }, error => {
                this.errorHandlerService.handleError(error);
                this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
            });
          }
        }).catch(() => {});
  }

  removeInvitation(id: number) {
    this.confirmationDialogService
      .confirm('Delete User', 'Do you really want to delete the user?')
        .then(result => {
          if (result) {
            this.invitationService.delete(id).subscribe(() => {
              const invitation = this.group.invitations.find(i => i.id === id);
              const index = this.group.invitations.indexOf(invitation);
              this.group.invitations.splice(index, 1);
              this.refressInvitations();
            }, error => {
              this.errorHandlerService.handleError(error);
              this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
            });
          }
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
