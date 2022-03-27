using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class GroupNotFoundException: Exception
    {
        public GroupNotFoundException() : base() { }

        public GroupNotFoundException(string message) : base(message) { }

        public GroupNotFoundException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
