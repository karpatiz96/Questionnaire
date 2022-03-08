using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Bll.Exceptions
{
    public class UserGroupNotFoundExcetpion: Exception
    {
        public UserGroupNotFoundExcetpion() : base() { }

        public UserGroupNotFoundExcetpion(string message) : base(message) { }

        public UserGroupNotFoundExcetpion(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
