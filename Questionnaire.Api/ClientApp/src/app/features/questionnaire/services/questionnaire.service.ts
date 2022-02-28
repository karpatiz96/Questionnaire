import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { QuestionnaireQuestionDto } from '../models/questionnaireQuestionDto';
import { QuestionnaireStartDto } from '../models/questionnaireStartDto';
import { QuestionnaireResultDto } from '../models/result/questionnaireResultDto';
import { QuestionnaireResultListDto } from '../models/result/questionnaireResultListDto';
import { UserQuestionnaireAnswerDetailsDto } from '../models/result/userQuestionnaireAnswerDetailsDto';
import { UserQuestionnaireAnswerDto } from '../models/userQuestionnaireAnswerDto';

@Injectable({
  providedIn: 'root'
})
export class QuestionnaireService {
  private baseUrl = '';

  constructor(private http: HttpClient) {
    this.baseUrl = 'https://localhost:5001/api';
   }

   start(id: number) {
       return this.http.post(`${this.baseUrl}/questionnaire/start`, id);
   }

   getStartById(id: number) {
       return this.http.get<QuestionnaireStartDto>(`${this.baseUrl}/questionnaire/start/${id}`);
   }

   answer(answerDto: UserQuestionnaireAnswerDto) {
     return this.http.post(`${this.baseUrl}/question/answer`, answerDto);
   }

   //Remove
   /*getQuestionnaireAnswerQuestionById(questionId: number) {
     return this.http.get<QuestionnaireQuestionDto>(`${this.baseUrl}/question/answer/${questionId}`);
   }*/

   getUserQuestionnaireAnswerById(userQuestionnaireAnswerId: number) {
    return this.http.get<UserQuestionnaireAnswerDetailsDto>(`${this.baseUrl}/question/answer/${userQuestionnaireAnswerId}`);
  }

   getQuestionnaireQuestions(questionnaireId: number) {
     return this.http.get<QuestionnaireQuestionDto[]>(`${this.baseUrl}/question/list/${questionnaireId}`);
   }

   getQuestionnaireResultAdmin(questionnaireId: number) {
     return this.http.get<QuestionnaireResultListDto>(`${this.baseUrl}/questionnaire/result/admin/${questionnaireId}`);
   }

   getQuestionnaireResult(userQuestionnaireId: number) {
     return this.http.get<QuestionnaireResultDto>(`${this.baseUrl}/questionnaire/result/${userQuestionnaireId}`);
   }
}
