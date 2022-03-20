export interface QuestionnaireResultListDto {
    id: number;
    title: string;
    description: string;
    begining: Date;
    finish: Date;
    questions: number;
    solved: number;
    members: number;
    results: QuestionnaireResultHeaderDto[];
}

export interface QuestionnaireResultHeaderDto {
    id: number;
    userName: string;
    maximumPoints: number;
    points: number;
    start: Date;
    completedTime: Date;
    completed: boolean;
}
