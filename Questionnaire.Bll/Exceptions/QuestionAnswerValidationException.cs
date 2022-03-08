using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class QuestionAnswerValidationException: Exception
    {
        public QuestionAnswerValidationException() : base() { }

        public QuestionAnswerValidationException(string message) : base(message) { }

        public QuestionAnswerValidationException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
