using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
