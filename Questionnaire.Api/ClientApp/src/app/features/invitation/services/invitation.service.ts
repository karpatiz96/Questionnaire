import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { InvitationDto } from '../models/invitationDto';
import { InvitationNewDto } from '../models/invitationNewDto';

@Injectable({
  providedIn: 'root'
})
export class InvitationService {
  private baseUrl = '';

  constructor(private http: HttpClient) {
    this.baseUrl = 'https://localhost:5001/api';
   }

   getInvitations() {
    return this.http.get<InvitationDto[]>(`${this.baseUrl}/invitation`);
   }

   create(invitation: InvitationNewDto) {
     return this.http.post<InvitationNewDto>(`${this.baseUrl}/invitation`, invitation);
   }

   accept(invitationId: number) {
     return this.http.get<InvitationDto>(`${this.baseUrl}/invitation/accept/${invitationId}`);
   }

   decline(invitationId: number) {
    return this.http.get<InvitationDto>(`${this.baseUrl}/invitation/decline/${invitationId}`);
   }

   delete(invitationId: number) {

   }
}
