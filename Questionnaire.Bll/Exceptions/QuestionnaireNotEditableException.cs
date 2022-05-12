using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class QuestionnaireNotEditableException: Exception
    {
        public QuestionnaireNotEditableException() : base() { }

        public QuestionnaireNotEditableException(string message) : base(message) { }

        public QuestionnaireNotEditableException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
