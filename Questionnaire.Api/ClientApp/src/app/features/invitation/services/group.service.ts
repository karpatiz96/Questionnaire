import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GroupListDto } from '../models/groupListDto';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private baseUrl = '';

  constructor(private http: HttpClient) {
    this.baseUrl = 'https://localhost:5001/api';
   }

   getInvitationGroups() {
     return this.http.get<GroupListDto[]>(`${this.baseUrl}/group/list`);
   }
}
