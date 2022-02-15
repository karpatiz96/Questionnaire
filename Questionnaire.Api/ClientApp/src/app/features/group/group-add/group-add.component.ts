import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-group-add',
  templateUrl: './group-add.component.html',
  styleUrls: ['./group-add.component.css']
})
export class GroupAddComponent implements OnInit {
  groupForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private groupService: GroupService) { }

  ngOnInit() {
    this.groupForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  get form() {
    return this.groupForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    if (this.groupForm.invalid) {
      return;
    }

    this.loading = true;
    this.groupService.create(this.groupForm.value)
      .subscribe(
        result => {
          this.router.navigate(['/group']);
        },
        error => {
          this.error = error;
          console.error(error);
          this.loading = false;
      });
  }

}
