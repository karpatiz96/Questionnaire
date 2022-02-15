import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { GroupDetailsDto } from '../models/groupDetailsDto';
import { GroupDto } from '../models/groupDto';
import { GroupHeaderDto } from '../models/groupHeaderDto';
import { GroupMemberDto } from '../models/groupMemberDto';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private baseUrl = '';

  constructor(private http: HttpClient) {
    this.baseUrl = 'https://localhost:5001/api';
   }

   getGroups() {
     return this.http.get<GroupHeaderDto[]>(`${this.baseUrl}/group`);
   }

   getList() {
    return this.http.get<GroupHeaderDto[]>(`${this.baseUrl}/group/me`);
   }

   getMembers(id: number) {
    return this.http.get<GroupMemberDto>(`${this.baseUrl}/group/member/${id}`);
   }

   getById(id: number) {
    return this.http.get<GroupDetailsDto>(`${this.baseUrl}/group/${id}`);
   }

   create(groupDto: GroupDto) {
     return this.http.post<GroupDto>(`${this.baseUrl}/group`, groupDto);
   }

   update(id: number, groupDto: GroupDto) {
    groupDto.id = id;
    return this.http.put(`${this.baseUrl}/group/${id}`, groupDto);
   }
}
