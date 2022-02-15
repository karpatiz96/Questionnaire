import { InvitationGroupDto } from './invitationGroupDto';
import { UserGroupDto } from './userGroupDto';

export interface GroupMemberDto {
    id: number;
    name: String;
    invitations: InvitationGroupDto[];
    users: UserGroupDto[];
}
