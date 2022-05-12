import { Component, OnInit } from '@angular/core';
import { GroupHeaderDto } from '../models/groupHeaderDto';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.css']
})
export class GroupListComponent implements OnInit {
  groupAll: GroupHeaderDto[];
  groups: GroupHeaderDto[] = [];

  page = 1;
  pageSize = 4;
  total = 0;

  constructor(private groupService: GroupService) { }

  ngOnInit() {
    this.loadGroups();
  }

  loadGroups() {
    this.groupService.getList()
      .subscribe(groups => {
        this.groupAll = groups;
        this.total = this.groupAll.length;
        this.refressGroups();
      }, error => {
        console.error(error);
      });
  }

  refressGroups() {
    this.groups = this.groupAll.slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
  }
}
