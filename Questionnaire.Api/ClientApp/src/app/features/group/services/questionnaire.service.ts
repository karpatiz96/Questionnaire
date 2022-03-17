import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { QuestionnaireDetailsDto } from '../../questionnaire/models/questionnaires/questionnaireDetailsDto';
import { QuestionnaireDto } from '../../questionnaire/models/questionnaires/questionnaireDto';
import { QuestionnaireHeaderDto } from '../models/questionnaireHeaderDto';
import { QuestionnaireListQueryDto } from '../models/questionnaireListQueryDto';

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

  getList(groupId: number, queryDto: QuestionnaireListQueryDto) {
    queryDto.groupId = groupId;
    const jsonData = JSON.stringify(queryDto);
    const params = new HttpParams({ fromObject: {
      groupId: queryDto.groupId.toString(),
      from: queryDto.from !== null ? queryDto.from.toString() : '',
      to:  queryDto.to !== null ? queryDto.to.toString() : '',
      visible: queryDto.visible ? 'true' : 'false'
    } });
    return this.http.get<QuestionnaireHeaderDto[]>(`${this.baseUrl}/questionnaire`, { params: params });
  }
}
