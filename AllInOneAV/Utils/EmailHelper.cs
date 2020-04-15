using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class EmailHelper
    {
        public static bool SendEmail(string subject, string content, string[] recev, string[] attachs)
        {
            Email email = new Email();
            email.mailFrom = "185776815@qq.com";
            email.mailPwd = "ejraknsegwklbggi";
            email.mailSubject = subject;
            email.mailBody = content;
            email.isbodyHtml = true;
            email.host = "smtp.qq.com";
            email.mailToArray = recev;
            email.attachmentsPath = attachs;

            return email.Send();
        }
    }
}
