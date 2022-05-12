export interface QuestionnaireHeaderDto {
    id: number;
    userQuestionnaireId: number;
    title: String;
    created: Date;
    begining: Date;
    finish: Date;
    start: Date | null;
    completedTime: Date | null;
    completed: boolean;
    evaluated: boolean;
    visibleToGroup: boolean;
}
