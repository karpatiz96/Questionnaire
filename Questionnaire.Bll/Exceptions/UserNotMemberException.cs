using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class UserNotMemberException: Exception
    {
        public UserNotMemberException() : base() { }

        public UserNotMemberException(string message) : base(message) { }

        public UserNotMemberException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
