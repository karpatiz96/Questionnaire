import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserGroupService {
  private baseUrl = '';

  constructor(private http: HttpClient) {
    this.baseUrl = 'https://localhost:5001/api';
   }

   makeAdmin(id: number) {
    return this.http.get(`${this.baseUrl}/usergroup/admin/${id}`);
   }

   makeUser(id: number) {
     return this.http.get(`${this.baseUrl}/usergroup/user/${id}`);
   }

   delete(id: number) {
     return this.http.delete(`${this.baseUrl}/usergroup/${id}`);
   }
}
