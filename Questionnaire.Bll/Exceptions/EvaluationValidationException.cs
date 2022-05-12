using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class EvaluationValidationException: Exception
    {
        public EvaluationValidationException() : base() { }

        public EvaluationValidationException(string message) : base(message) { }

        public EvaluationValidationException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
