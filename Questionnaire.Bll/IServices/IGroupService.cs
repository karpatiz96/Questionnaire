using Questionnaire.Bll.Dtos;
using Questionnaire.Dll.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.IServices
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupHeaderDto>> GetGroups(string userId);

        Task<IEnumerable<GroupHeaderDto>> GetMyGroups(string userId);

        Task<IEnumerable<GroupListDto>> GetGroupsList(string userId);

        Task<GroupMemberDto> GetGroupMembers(string userId, int groupId);

        Task<GroupDetailsDto> GetGroup(string userId, int groupId);

        Task<GroupDto> GetGroupById(string userId, int groupId); 

        Task<GroupDto> CreateGroup(GroupDto groupDto, string userId);

        Task UpdateGroup(string userId, GroupDto groupDto);
    }
}
