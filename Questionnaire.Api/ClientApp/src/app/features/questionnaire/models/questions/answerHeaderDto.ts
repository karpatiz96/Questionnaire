import { AnswerType } from '../answers/answerDto';

export interface AnswerHeaderDto {
    id: number;
    name: String;
    type: AnswerType;
    value: number;
}
