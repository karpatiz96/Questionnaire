import { Component, OnInit } from '@angular/core';
import { error } from 'selenium-webdriver';
import { InvitationDto } from '../models/invitationDto';
import { InvitationService } from '../services/invitation.service';

@Component({
  selector: 'app-invitation',
  templateUrl: './invitation.component.html',
  styleUrls: ['./invitation.component.css']
})
export class InvitationComponent implements OnInit {
  invitations: InvitationDto[];
  Invitations = ['Undecided', 'Accepted', 'Declined'];

  constructor(private invitationService: InvitationService) { }

  ngOnInit() {
    this.loadInvitations();
  }

  loadInvitations() {
    this.invitationService.getInvitations()
      .subscribe(invitations => {
        this.invitations = invitations;
      }, error => {
        console.error(error);
      });
  }

  accept(id: number) {
    this.invitationService.accept(id)
    .subscribe(result => {
      const old = this.invitations.find(invitation => invitation.id === result.id);
      const index = this.invitations.indexOf(old);
      this.invitations[index] = result;
    });
  }

  decline(id: number) {
    this.invitationService.decline(id)
    .subscribe(result => {
      const old = this.invitations.find(invitation => invitation.id === result.id);
      const index = this.invitations.indexOf(old);
      this.invitations[index] = result;
    });
  }

}
