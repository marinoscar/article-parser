using api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Provider
{
    public class ContentRepository
    {
        public ContentRepository()
        {
            DataContext = new MongoDataContext("kapp");
        }

        public ContentRepository(IDataContext context)
        {
            DataContext = context;
        }

        public IDataContext DataContext { get; set; }

        public void PersistResult(ParserResult item)
        {
            DataContext.Insert<ParserResult>("contents", item);
        }

        public ParserResult GetResult(string postId)
        {
            var filter = Builders<ParserResult>.Filter.Eq("Id", postId);
            return DataContext.Select<ParserResult>("contents", filter).FirstOrDefault() ;
        }
    }
}
