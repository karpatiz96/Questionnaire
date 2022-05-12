import { QuestionType } from 'src/app/features/questionnaire/models/questionnaires/questionHeaderDto';
import { QuestionAnswerResultDto } from './questionAnswerResultDto';

export interface UserQuestionnaireAnswerDetailsDto {
    id: number;
    questionnaireTitle: string;
    questionTitle: string;
    description: string;
    type: QuestionType;
    userAnswer: string;
    answerId: number;
    maximumPoints: number;
    points: number;
    evaluated: boolean;
    finished: Date;
    completed: boolean;
    userName: String;
    answers: QuestionAnswerResultDto[];
    role: string;
}
