import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { QuestionnaireDetailsDto } from '../models/questionnaires/questionnaireDetailsDto';
import { QuestionnaireDto } from '../models/questionnaires/questionnaireDto';

@Injectable({
  providedIn: 'root'
})
export class QuestionnaireService {
  private baseUrl = '';

  constructor(private http: HttpClient) {
    this.baseUrl = 'https://localhost:5001/api';
   }

   create(questionnaireDto: QuestionnaireDto, groupId: number) {
    questionnaireDto.groupId = groupId;
    return this.http.post<QuestionnaireDto>(`${this.baseUrl}/questionnaire`, questionnaireDto);
  }

  getDetailsById(id: number) {
    return this.http.get<QuestionnaireDetailsDto>(`${this.baseUrl}/questionnaire/${id}`);
  }

  getById(id: number) {
    return this.http.get<QuestionnaireDto>(`${this.baseUrl}/questionnaire/update/${id}`);
  }

  update(id: number, questionnaireDto: QuestionnaireDto) {
    questionnaireDto.id = id;
    return this.http.put(`${this.baseUrl}/questionnaire/${id}`, questionnaireDto);
  }

  hide(id: number) {
    return this.http.post(`${this.baseUrl}/questionnaire/hide`, id);
  }

  show(id: number) {
    return this.http.post(`${this.baseUrl}/questionnaire/show`, id);
  }

  copy(id: number) {
    return this.http.post<QuestionnaireDto>(`${this.baseUrl}/questionnaire/copy`, id);
  }
}
