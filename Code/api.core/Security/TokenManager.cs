using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.core.Security
{
    public static class TokenManager
    {
        private static List<string> _tokens;

        public static  bool IsValid(string token)
        {
            if (_tokens == null) LoadTokens();
            return _tokens.Contains(token);
        }

        private static void LoadTokens()
        {
            _tokens = new List<string>();
            var tokenData = ConfigurationManager.AppSettings["tokens"];
            _tokens = tokenData.Split(",".ToCharArray()).ToList();
        }
    }
}
