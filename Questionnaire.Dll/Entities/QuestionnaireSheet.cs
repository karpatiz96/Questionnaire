using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnaire.Dll.Entities
{
    public class QuestionnaireSheet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool RandomQuestionOrder { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastEdited { get; set; }

        public DateTime Begining { get; set; }

        public DateTime Finish { get; set; }

        public bool VisibleToGroup { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public ICollection<Question> Questions { get; set; }

        //Solved
        public ICollection<UserQuestionnaire> UserQuestionnaires { get; set; }
    }
}
