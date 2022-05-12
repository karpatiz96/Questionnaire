import { QuestionAnswerDto } from './questionAnswerDto';
import { QuestionType } from './questionType';

export interface QuestionListDto {
    id: number;
    title: string;
    description: string;
    type: QuestionType;
    points: number;
    answers: QuestionAnswerDto[];
}
