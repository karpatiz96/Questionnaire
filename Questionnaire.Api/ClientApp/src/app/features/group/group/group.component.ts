import { Component, OnInit } from '@angular/core';
import { GroupHeaderDto } from '../models/groupHeaderDto';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent implements OnInit {
  groups: GroupHeaderDto[];

  constructor(private groupService: GroupService) { }

  ngOnInit() {
    this.loadGroups();
  }

  loadGroups() {
    this.groupService.getGroups()
      .subscribe(groups => {
        this.groups = groups;
      }, error => {
        console.error(error);
      });
  }
}
