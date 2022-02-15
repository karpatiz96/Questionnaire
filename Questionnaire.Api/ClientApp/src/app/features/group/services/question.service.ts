import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { QuestionDetailsDto } from '../models/questionDetailsDto';
import { QuestionDto } from '../models/questionDto';
import { QuestionType } from '../models/questionHeaderDto';

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

  update(questionDto: QuestionDto, questionId: number) {
    questionDto.id = questionId;
    questionDto.type = Number(questionDto.type);
    return this.http.put(`${this.baseUrl}/question/${questionDto.id}`, questionDto);
  }
}
