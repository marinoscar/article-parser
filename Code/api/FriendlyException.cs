using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api
{
    public class FriendlyException : Exception
    {
        public FriendlyException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
