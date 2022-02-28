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

export interface QuestionAnswerDto {
    id: number;
    questionId: number;
    name: String;
}

export enum QuestionType {
    TrueOrFalse = 0,
    MultipleChoice = 1,
    FreeText = 2,
    ConcreteText = 3
}
