import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { GroupHeaderDto } from '../../group/models/groupHeaderDto';
import { GroupService } from '../../group/services/group.service';
import { InvitationService } from '../services/invitation.service';

@Component({
  selector: 'app-invitation-add',
  templateUrl: './invitation-add.component.html',
  styleUrls: ['./invitation-add.component.css']
})
export class InvitationAddComponent implements OnInit {
  invitationForm: FormGroup;
  groups: GroupHeaderDto[];
  loading = false;
  submitted = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private groupService: GroupService,
    private invitationService: InvitationService) { }

  ngOnInit() {
    this.invitationForm = this.formBuilder.group({
      email: ['', Validators.required],
      groupId: [0, Validators.required]
    });

    this.groupService.getGroups().subscribe(result => {
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
          this.error = error;
          console.error(error);
          this.loading = false;
      });
  }

}
