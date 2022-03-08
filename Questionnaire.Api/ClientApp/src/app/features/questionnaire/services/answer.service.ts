import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AnswerDetailsDto } from '../models/answers/answerDetailsDto';
import { AnswerDto } from '../models/answers/answerDto';

@Injectable({
  providedIn: 'root'
})
export class AnswerService {
  private baseUrl = '';

  constructor(private http: HttpClient) {
    this.baseUrl = 'https://localhost:5001/api';
   }

   create(answerDto: AnswerDto, questionId: number) {
    answerDto.questionId = questionId;
    answerDto.type = Number(answerDto.type);
    return this.http.post<AnswerDto>(`${this.baseUrl}/answer`, answerDto);
  }

  getById(id: number) {
    return this.http.get<AnswerDetailsDto>(`${this.baseUrl}/answer/${id}`);
  }

  update(answerDto: AnswerDto, answerId: number) {
    answerDto.id = answerId;
    answerDto.type = Number(answerDto.type);
    return this.http.put(`${this.baseUrl}/answer/${answerDto.id}`, answerDto);
  }

  delete(id: number) {
    return this.http.delete(`${this.baseUrl}/answer/${id}`);
  }
}
