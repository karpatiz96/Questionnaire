using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class UserNotAdminException : Exception
    {
        public UserNotAdminException() : base() { }

        public UserNotAdminException(string message) : base(message) { }

        public UserNotAdminException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
