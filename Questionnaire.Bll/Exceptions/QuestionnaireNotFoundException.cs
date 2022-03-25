using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class QuestionnaireNotFoundException: Exception
    {
        public QuestionnaireNotFoundException() : base() { }

        public QuestionnaireNotFoundException(string message) : base(message) { }

        public QuestionnaireNotFoundException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
