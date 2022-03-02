export interface AnswerDto {
    id: number;
    questionId: number;
    name: String;
    userAnswer: String;
    type: AnswerType;
    value: number;
}

export enum AnswerType {
    Correct = 0,
    False = 1
}
