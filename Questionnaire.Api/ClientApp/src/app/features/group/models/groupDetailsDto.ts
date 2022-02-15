export interface GroupDetailsDto {
    id: number;
    name: String;
    groupAdmin: String;
    description: String;
    created: Date;
    lastPost: Date;
    members: number;
}
