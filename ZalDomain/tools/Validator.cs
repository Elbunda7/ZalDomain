using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.tools
{
    public static class Validator
    {
        public static bool IsValidEmail(string email) {
            try {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }
    }
}
