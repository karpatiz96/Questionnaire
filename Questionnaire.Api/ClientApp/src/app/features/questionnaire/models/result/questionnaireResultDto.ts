import { QuestionType } from '../questionnaireQuestionDto';

export interface QuestionnaireResultDto {
    id: number;
    userName: string;
    title: string;
    description: string;
    begining: Date;
    finish: Date;
    questions: number;
    maximumPoints: number;
    points: number;
    answers: UserQuestionAnswerHeaderDto[];
}

export interface UserQuestionAnswerHeaderDto {
    id: number;
    index: number;
    name: string;
    type: QuestionType;
    maximumPoints: number;
    points: number;
    evaluated: boolean;
    finished: Date;
    completed: boolean;
}
