using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Provider
{
    public class ContentRepository
    {
        public ContentRepository(IDataContext context)
        {
            DataContext = context;
        }

        public IDataContext DataContext { get; set; }

        public void PersistResult(ParserResult item)
        {
            var entity = new ParserResultEntity(item);
            DataContext.Insert<ParserResultEntity>(TableMap.Contents, new ParserResultEntity[] { entity });
        }
    }
}
