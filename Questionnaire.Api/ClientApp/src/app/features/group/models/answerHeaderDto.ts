import { AnswerType } from './answerDto';

export interface AnswerHeaderDto {
    id: number;
    name: String;
    type: AnswerType;
    value: number;
}
