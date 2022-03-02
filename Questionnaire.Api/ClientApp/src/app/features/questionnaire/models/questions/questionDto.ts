import { QuestionType } from '../questionnaires/questionHeaderDto';

export interface QuestionDto {
    id: number;
    questionnaireId: number;
    number: number;
    name: String;
    description: String;
    type: QuestionType;
    value: number;
    suggestedTime: number;
}
