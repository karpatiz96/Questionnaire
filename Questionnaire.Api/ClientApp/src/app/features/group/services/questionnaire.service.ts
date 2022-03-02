import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { QuestionnaireDetailsDto } from '../../questionnaire/models/questionnaires/questionnaireDetailsDto';
import { QuestionnaireDto } from '../../questionnaire/models/questionnaires/questionnaireDto';

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

  getById(id: number) {
    return this.http.get<QuestionnaireDetailsDto>(`${this.baseUrl}/questionnaire/${id}`);
  }

  update(id: number, questionnaireDto: QuestionnaireDto) {
    questionnaireDto.id = id;
    return this.http.put(`${this.baseUrl}/questionnaire/${id}`, questionnaireDto);
  }
}
