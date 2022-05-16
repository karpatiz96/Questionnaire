export interface GroupHeaderDto {
    id: number;
    name: String;
    created: Date;
    lastPost: Date;
    members: number;
    lastPostName: String;
    questionnairePosted: boolean;
}
