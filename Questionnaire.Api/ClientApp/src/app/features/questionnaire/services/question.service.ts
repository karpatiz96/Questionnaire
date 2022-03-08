import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { QuestionType } from '../../questionnaire/models/questionnaires/questionHeaderDto';
import { QuestionDetailsDto } from '../models/questions/questionDetailsDto';
import { QuestionDto } from '../models/questions/questionDto';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  private baseUrl = '';

  constructor(private http: HttpClient) {
    this.baseUrl = 'https://localhost:5001/api';
   }

   create(questionDto: QuestionDto, questionnaireId: number) {
    questionDto.questionnaireId = questionnaireId;
    questionDto.type = Number(questionDto.type);
    return this.http.post<QuestionDto>(`${this.baseUrl}/question`, questionDto);
  }

  getById(id: number) {
    return this.http.get<QuestionDetailsDto>(`${this.baseUrl}/question/${id}`);
  }

  update(questionDto: QuestionDto, id: number) {
    questionDto.id = id;
    questionDto.type = Number(questionDto.type);
    return this.http.put(`${this.baseUrl}/question/${questionDto.id}`, questionDto);
  }

  delete(id: number) {
    return this.http.delete(`${this.baseUrl}/question/${id}`);
  }
}
