using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Commons.Exeptions
{
    public class UserNotificationException : Exception
    {
        public UserNotificationException(string message)
            : base(message) { }

        public UserNotificationException(string message, Exception inner)
            : base(message, inner) { }
    }
}
