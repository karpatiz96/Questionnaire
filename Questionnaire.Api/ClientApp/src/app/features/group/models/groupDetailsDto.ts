import { QuestionnaireHeaderDto } from './questionnaireHeaderDto';

export interface GroupDetailsDto {
    id: number;
    name: String;
    groupRole: String;
    groupAdmin: String;
    description: String;
    created: Date;
    lastPost: Date;
    members: number;
    questionnaires: QuestionnaireHeaderDto[];
}
