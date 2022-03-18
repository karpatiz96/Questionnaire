import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-group-edit',
  templateUrl: './group-edit.component.html',
  styleUrls: ['./group-edit.component.css']
})
export class GroupEditComponent implements OnInit {
  groupForm: FormGroup;
  groupId: number;
  loading = false;
  submitted = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private groupService: GroupService,
    private errorHandlerService: ErrorHandlerService) { }

  ngOnInit() {
    this.groupForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: ['', Validators.required]
    });

    this.route.params.subscribe(params => {
      this.loadGroup(params['id']);
    });
  }

  private loadGroup(id: number) {
    this.groupService.getById(id)
      .subscribe(result => {
        this.groupForm.patchValue({
          name: result.name,
          description: result.description
        });
      }, error => {
        this.errorHandlerService.handleError(error);
      });

    this.groupId = id;
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
    this.groupService.update(this.groupId, this.groupForm.value)
      .subscribe(
        result => {
          this.router.navigate(['/group']);
        },
        error => {
          this.errorHandlerService.handleError(error);
          this.loading = false;
      });
  }

}
