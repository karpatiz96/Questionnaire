export interface GroupDetailsDto {
    id: number;
    name: String;
    groupRole: String;
    description: String;
    created: Date;
    lastPost: Date;
    members: number;
}
