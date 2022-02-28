export interface QuestionnaireStartDto {
    id: number;
    title: string;
    description: string;
    begining: Date;
    finish: Date;
    questions: number;
}
