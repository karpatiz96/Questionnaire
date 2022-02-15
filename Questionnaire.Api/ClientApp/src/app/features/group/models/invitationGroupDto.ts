import { InvitationStatus } from '../../models/invitationStatus';

export interface InvitationGroupDto {
    id: number;
    userId: String;
    userName: String;
    date: Date;
    status: InvitationStatus;
}
