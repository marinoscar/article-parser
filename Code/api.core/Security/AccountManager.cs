using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.core.Security
{
    public class AccountManager : IAccountManager
    {
        public User GetCurrent()
        {
            return new User()
            {
                Id = ConfigurationManager.AppSettings["user.id"]
            };
        }
    }
}
