using api.Provider;
using api.Security;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api
{
    public class Map
    {
        private static Map _i;
        private IContainer _container;
        public Map()
        {
            _container = new Container(i => {
                i.For<IDataContext>().Use<MongoDataContext>();
                i.For<IAccountManager>().Use<AccountManager>();
            });

        }

        public IContainer Container { get { return _container; } }

        public static Map I { get { return _i ?? (_i = new Map()); } }
    }
}
