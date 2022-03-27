using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class InvitationNotFoundException: Exception
    {
        public InvitationNotFoundException() : base() { }

        public InvitationNotFoundException(string message) : base(message) { }

        public InvitationNotFoundException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
