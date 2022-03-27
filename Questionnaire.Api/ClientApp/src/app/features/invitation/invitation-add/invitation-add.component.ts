import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertService } from '../../shared/services/alert.service';
import { ErrorHandlerService } from '../../shared/services/error-handler.service';
import { GroupListDto } from '../models/groupListDto';
import { GroupService } from '../services/group.service';
import { InvitationService } from '../services/invitation.service';

@Component({
  selector: 'app-invitation-add',
  templateUrl: './invitation-add.component.html',
  styleUrls: ['./invitation-add.component.css']
})
export class InvitationAddComponent implements OnInit {
  invitationForm: FormGroup;
  groups: GroupListDto[];
  loading = false;
  submitted = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private groupService: GroupService,
    private invitationService: InvitationService,
    private errorHandlerService: ErrorHandlerService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.invitationForm = this.formBuilder.group({
      email: ['', Validators.required],
      groupId: [0, Validators.required]
    });

    this.groupService.getInvitationGroups().subscribe(result => {
      this.groups = result;
    });
  }

  get form() {
    return this.invitationForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    if (this.invitationForm.invalid) {
      return;
    }

    this.loading = true;
    this.invitationService.create(this.invitationForm.value)
      .subscribe(
        result => {
          this.router.navigate(['/invitation']);
        },
        error => {
          this.errorHandlerService.handleError(error);
          this.alertService.error(this.errorHandlerService.errorMessage, { id: 'alert-1' });
          this.loading = false;
          console.log(error);
      });
  }

}
