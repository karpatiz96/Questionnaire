import { InvitationStatus } from '../../models/invitationStatus';

export interface InvitationDto {
    id: number;
    groupId: number;
    groupName: String;
    userId: String;
    Email: String;
    groupCreated: Date;
    date: Date;
    adminName: String;
    status: InvitationStatus;
}
