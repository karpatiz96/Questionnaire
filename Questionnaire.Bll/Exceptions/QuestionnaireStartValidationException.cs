using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class QuestionnaireStartValidationException : Exception
    {
        public QuestionnaireStartValidationException() : base() { }

        public QuestionnaireStartValidationException(string message) : base(message) { }

        public QuestionnaireStartValidationException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
