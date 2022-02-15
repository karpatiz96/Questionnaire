using Microsoft.EntityFrameworkCore;
using Questionnaire.Bll.Dtos;
using Questionnaire.Bll.IServices;
using Questionnaire.Dll;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Questionnaire.Dll.Entities.Invitation;

namespace Questionnaire.Bll.Services
{
    public class InvitationService : IInvitationService
    {
        private QuestionnaireDbContext _dbContext;

        public InvitationService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static Expression<Func<Invitation, InvitationDto>> InvitationSelector { get; } = g => new InvitationDto
        {
            Id = g.Id,
            UserId = g.UserId,
            Email = g.User.Email,
            GroupId = g.GroupId,
            GroupName = g.Group.Name,
            AdminName = g.Group.UserGroups.FirstOrDefault(ug => ug.MainAdmin).User.UserName,
            GroupCreated = g.Group.Created,
            Status = g.Status,
            Date = g.Created
        };

        public async Task<InvitationNewDto> CreateInvitation(InvitationNewDto invitationNewDto, User user)
        {
            var invitation = new Invitation
            {
                UserId = user.Id,
                GroupId = invitationNewDto.GroupId,
                Created = DateTime.Now,
                Status = InvitationStatus.Undecided
            };

            _dbContext.Invitations.Add(invitation);

            await _dbContext.SaveChangesAsync();

            return invitationNewDto;
        }

        public async Task<Invitation> DeleteInvitation(int id)
        {
            var invitation = await _dbContext.Invitations
                .FirstOrDefaultAsync(i => i.Id == id);

            var user = await _dbContext.Users
                .Include(u => u.Invitations)
                .Where(u => u.Id == invitation.UserId)
                .FirstOrDefaultAsync();

            user.Invitations.Remove(invitation);

            var group = await _dbContext.Groups
                .Include(g => g.Invitations)
                .Where(g => g.Id == invitation.GroupId)
                .FirstOrDefaultAsync();

            group.Invitations.Remove(invitation);

            _dbContext.Invitations.Remove(invitation);

            await _dbContext.SaveChangesAsync();

            return invitation;
        }

        public async Task<IEnumerable<InvitationDto>> GetInvitations(string userId)
        {
            var invitation = await _dbContext.Invitations
                .Include(i => i.Group)
                    .ThenInclude(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                .Include(i => i.User)
                .Where(i => i.UserId == userId)
                .Select(InvitationSelector)
                .ToListAsync();

            return invitation;
        }

        public async Task<InvitationDto> AcceptInvitation(int invitationId, InvitationStatus status)
        {
            var invitation = await _dbContext.Invitations
                .Where(i => i.Id == invitationId)
                .FirstOrDefaultAsync();

            invitation.Status = status;

            _dbContext.Attach(invitation).State = EntityState.Modified;

            var userGroup = new UserGroup
            {
                UserId = invitation.UserId,
                GroupId = invitation.GroupId,
                Role = "User",
                MainAdmin = false
            };

            _dbContext.UserGroups.Add(userGroup);

            await _dbContext.SaveChangesAsync();

            var invitationDto = await _dbContext.Invitations
                .Include(i => i.Group)
                    .ThenInclude(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                .Include(i => i.User)
                .Where(i => i.Id == invitationId)
                .Select(InvitationSelector)
                .FirstOrDefaultAsync();

            return invitationDto;
        }

        public async Task<InvitationDto> DeclineInvitation(int invitationId, InvitationStatus status)
        {
            var invitation = await _dbContext.Invitations
                .Where(i => i.Id == invitationId)
                .FirstOrDefaultAsync();

            invitation.Status = status;

            _dbContext.Attach(invitation).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var invitationDto = await _dbContext.Invitations
                .Include(i => i.Group)
                    .ThenInclude(g => g.UserGroups)
                        .ThenInclude(ug => ug.User)
                .Include(i => i.User)
                .Where(i => i.Id == invitationId)
                .Select(InvitationSelector)
                .FirstOrDefaultAsync();

            return invitationDto;
        }
    }
}
