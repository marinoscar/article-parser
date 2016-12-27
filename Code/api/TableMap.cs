using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api
{
    public static class TableMap
    {
        public static string Contents { get { return ConfigurationManager.AppSettings["azure.table.contents"];  } }
    }
}
