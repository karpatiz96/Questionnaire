using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class AnswerNotFoundException: Exception
    {
        public AnswerNotFoundException() : base() { }

        public AnswerNotFoundException(string message) : base(message) { }

        public AnswerNotFoundException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
