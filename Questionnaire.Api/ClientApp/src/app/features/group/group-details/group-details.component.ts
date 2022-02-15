import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { GroupDetailsDto } from '../models/groupDetailsDto';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.css']
})
export class GroupDetailsComponent implements OnInit {
  group: GroupDetailsDto = {
    id: 0,
    description: 'Test',
    name: 'Test Group',
    created: new Date(2021, 1, 1),
    lastPost: new Date(2021, 1, 1),
    groupAdmin: 'Admin',
    members: 1
  };

  constructor(
    private route: ActivatedRoute,
    private groupService: GroupService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.loadGroup(params['id']);
    });
  }

  loadGroup(id: number) {
    this.groupService.getById(id)
    .pipe(first()).subscribe(result => {
      this.group = result;
    });
  }

}
