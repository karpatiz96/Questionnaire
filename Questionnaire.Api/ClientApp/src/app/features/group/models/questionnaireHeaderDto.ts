export interface QuestionnaireHeaderDto {
    id: number;
    userQuestionnaireId: number;
    title: String;
    created: Date;
    begining: Date;
    finish: Date;
    visibleToGroup: boolean;
}
