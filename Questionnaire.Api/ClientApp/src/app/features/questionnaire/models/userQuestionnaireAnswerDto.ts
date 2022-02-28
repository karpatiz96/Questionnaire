export class UserQuestionnaireAnswerDto {
    id: number;
    questionId: number;
    answerId: number;
    userAnswer: string;

    constructor(id: number, questionId: number, answerId: number, userAnswer: string) {
        this.id = id;
        this.questionId = questionId;
        this.answerId = answerId;
        this.userAnswer = userAnswer;
    }
}
