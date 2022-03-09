import { AnswerType } from './answerDto';
import { QuestionType } from '../questionnaires/questionHeaderDto';

export interface AnswerDetailsDto {
    id: number;
    questionType: QuestionType;
    type: AnswerType;
    name: String;
    userAnswer: String;
    value: number;
    visibleToGroup: boolean;
}
