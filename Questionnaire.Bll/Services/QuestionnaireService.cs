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

namespace Questionnaire.Bll.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private QuestionnaireDbContext _dbContext;

        public QuestionnaireService(QuestionnaireDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static Expression<Func<QuestionnaireSheet, QuestionnaireDetailsDto>> QuestionnaireDetailsSelector { get; } = q => 
        new QuestionnaireDetailsDto
        {
            Id = q.Id,
            Title = q.Name,
            Description = q.Description,
            Begining = q.Begining,
            Finish = q.Finish,
            VisibleToGroup = q.VisibleToGroup,
            Created = q.Created,
            LastEdited = q.LastEdited,
            Questions = q.Questions.Select(q => new QuestionHeaderDto
            {
                Id = q.Id,
                Name = q.Name,
                Number = q.Number,
                Type = q.Type,
                Value = q.MaximumPoints
            }).ToList()
        };

        public static Expression<Func<QuestionnaireSheet, QuestionnaireHeaderDto>> QuestionnaireHeaderSelector { get; } = q =>
        new QuestionnaireHeaderDto
        {
            Id = q.Id,
            Title = q.Name,
            Begining = q.Begining,
            Finish = q.Finish,
            VisibleToGroup = q.VisibleToGroup,
            Created = q.Created
        };

        public async Task<QuestionnaireDto> CreateQuestionnaire(QuestionnaireDto questionnaireDto)
        {
            var questionnaire = new QuestionnaireSheet
            {
                GroupId = questionnaireDto.GroupId,
                Name = questionnaireDto.Title,
                Description = questionnaireDto.Description,
                Begining = questionnaireDto.Begining,
                Finish = questionnaireDto.Finish,
                VisibleToGroup = questionnaireDto.VisibleToGroup,
                Created = DateTime.Now,
                LastEdited = DateTime.Now
            };

            var group = await _dbContext.Groups
                .Include(g => g.QuestionnaireSheets)
                .FirstOrDefaultAsync(g => g.Id == questionnaireDto.GroupId);

            group.QuestionnaireSheets.Add(questionnaire);

            await _dbContext.SaveChangesAsync();

            return questionnaireDto;
        }

        public async Task<QuestionnaireDetailsDto> GetQuestionnaire(int questionnaireId)
        {
            var questionnaire = await _dbContext.QuestionnaireSheets
                .Include(q => q.Questions)
                .Where(q => q.Id == questionnaireId)
                .Select(QuestionnaireDetailsSelector)
                .FirstOrDefaultAsync();

            return questionnaire;
        }

        public async Task<IEnumerable<QuestionnaireHeaderDto>> GetQuestionnaires(int groupId, string userId)
        {
            var questionnaires = await _dbContext.QuestionnaireSheets
                .Include(q => q.Group)
                    .ThenInclude(g => g.UserGroups)
                .Where(q => q.GroupId == groupId && q.Group.UserGroups.Any(ug => ug.UserId == userId))
                .Select(QuestionnaireHeaderSelector)
                .ToListAsync();

            return questionnaires;
        }

        public async Task<QuestionnaireDto> UpdateQuestionnaire(QuestionnaireDto questionnaireDto)
        {
            var questionnaire = await _dbContext.QuestionnaireSheets
                .FirstOrDefaultAsync(g => g.Id == questionnaireDto.Id);

            questionnaire.Name = questionnaireDto.Title;
            questionnaire.Description = questionnaireDto.Description;
            questionnaire.Begining = questionnaireDto.Begining;
            questionnaire.Finish = questionnaire.Finish;
            questionnaire.VisibleToGroup = questionnaireDto.VisibleToGroup;
            questionnaire.LastEdited = DateTime.Now;

            _dbContext.Attach(questionnaire).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return questionnaireDto;
        }
    }
}
