import { QuestionListDto } from './questionListDto';

export interface QuestionnaireQuestionListDto {
    id: number;
    title: string;
    limited: boolean;
    timeLimit: number;
    questions: QuestionListDto[];
}
