import { AnswerHeaderDto } from './answerHeaderDto';
import { QuestionDto } from './questionDto';

export interface QuestionDetailsDto extends QuestionDto {
    Answers: AnswerHeaderDto[];
}
