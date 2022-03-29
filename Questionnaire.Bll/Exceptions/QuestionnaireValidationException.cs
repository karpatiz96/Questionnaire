using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class QuestionnaireValidationException: Exception
    {
        public QuestionnaireValidationException() : base() { }

        public QuestionnaireValidationException(string message) : base(message) { }

        public QuestionnaireValidationException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
