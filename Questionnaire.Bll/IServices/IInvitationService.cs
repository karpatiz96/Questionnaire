using Questionnaire.Bll.Dtos;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Questionnaire.Dll.Entities.Invitation;

namespace Questionnaire.Bll.IServices
{
    public interface IInvitationService
    {
        Task<IEnumerable<InvitationDto>> GetInvitations(string userId);

        Task<InvitationNewDto> CreateInvitation(InvitationNewDto invitationNewDto, User user);

        Task<InvitationDto> AcceptInvitation(int invitationId, InvitationStatus status);

        Task<InvitationDto> DeclineInvitation(int invitationId, InvitationStatus status);

        Task<Invitation> DeleteInvitation(string userId, int Id);
    }
}
