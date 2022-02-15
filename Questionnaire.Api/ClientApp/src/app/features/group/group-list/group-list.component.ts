import { Component, OnInit } from '@angular/core';
import { GroupHeaderDto } from '../models/groupHeaderDto';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.css']
})
export class GroupListComponent implements OnInit {
  groups: GroupHeaderDto[];

  constructor(private groupService: GroupService) { }

  ngOnInit() {
    this.loadGroups();
  }

  loadGroups() {
    this.groupService.getList()
      .subscribe(groups => {
        this.groups = groups;
      }, error => {
        console.error(error);
      });
  }

}
