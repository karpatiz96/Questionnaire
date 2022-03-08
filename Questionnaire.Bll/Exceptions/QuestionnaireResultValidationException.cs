using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class QuestionnaireResultValidationException : Exception
    {
        public QuestionnaireResultValidationException() : base() { }

        public QuestionnaireResultValidationException(string message) : base(message) { }

        public QuestionnaireResultValidationException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
