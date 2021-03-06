export interface QuestionnaireDto {
    id: number;
    groupId: number;
    title: String;
    description: String;
    begining: Date;
    finish: Date;
    visibleToGroup: boolean;
    randomQuestionOrder: boolean;
    limited: boolean;
    timeLimit: number;
}
