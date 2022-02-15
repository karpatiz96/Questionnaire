import { AnswerType } from './answerDto';
import { QuestionType } from './questionHeaderDto';

export interface AnswerDetailsDto {
    id: number;
    questionType: QuestionType;
    type: AnswerType;
    name: String;
    userAnswer: String;
    value: number;
}
