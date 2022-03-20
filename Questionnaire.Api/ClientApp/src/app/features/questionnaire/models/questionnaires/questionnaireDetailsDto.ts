import { QuestionHeaderDto } from './questionHeaderDto';

export interface QuestionnaireDetailsDto {
    id: number;
    title: string;
    description: string;
    begining: Date;
    finish: Date;
    created: Date;
    lastEdited: Date;
    visibleToGroup: boolean;
    randomQuestionOrder: boolean;
    questions: QuestionHeaderDto[];
}
