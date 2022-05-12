import { QuestionAnswerDto } from './questionAnswerDto';
import { QuestionType } from './questionType';

export interface QuestionnaireQuestionDto {
    id: number;
    questionId: number;
    questionnaireTitle: string;
    questionTitle: string;
    description: string;
    type: QuestionType;
    points: number;
    answers: QuestionAnswerDto[];
}
