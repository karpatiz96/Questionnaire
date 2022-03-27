using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class InvitationValidationException: Exception
    {
        public InvitationValidationException() : base() { }

        public InvitationValidationException(string message) : base(message) { }

        public InvitationValidationException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
