using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class QuestionNotFoundException: Exception
    {
        public QuestionNotFoundException() : base() { }

        public QuestionNotFoundException(string message) : base(message) { }

        public QuestionNotFoundException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
