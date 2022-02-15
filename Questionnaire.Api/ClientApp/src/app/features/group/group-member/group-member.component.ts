import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { GroupMemberDto } from '../models/groupMemberDto';
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

  constructor(
    private route: ActivatedRoute,
    private groupService: GroupService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.loadGroupMembers(params['id']);
    });
  }

  loadGroupMembers(id: number) {
    this.groupService.getMembers(id)
    .pipe(first()).subscribe(result => {
      this.group = result;
    });
  }

  removeUser(id: number) {

  }

  updateUser(id: number) {

  }

  removeInvitation(id: number) {

  }

}
