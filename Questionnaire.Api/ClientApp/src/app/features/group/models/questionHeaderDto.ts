export interface QuestionHeaderDto {
    id: number;
    number: number;
    name: String;
    type: QuestionType;
    value: number;
}

export enum QuestionType {
    TrueOrFalse = 0,
    MultipleChoice = 1,
    FreeText = 2,
    ConcreteText = 3
}
