using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Models
{
    public class ParserResult : IParserResult
    {
        public ParserResult()
        {
            UtcCreatedOn = DateTime.UtcNow;
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string TitleHash { get; set; }
        public string Content { get; set; }
        public string ContentHash { get; set; }
        public string Author { get; set; }
        public string DatePublish { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public IEnumerable<string> Images { get; set; }
        public IEnumerable<string> Keywords { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public DateTime UtcCreatedOn { get; set; }
    }

    public class ParserResultEntity : TableEntity, IParserResult
    {
        public ParserResultEntity()
        {

        }

        public ParserResultEntity(IParserResult source)
        {
            RowKey = source.Id;
            PartitionKey = source.UserId;
            foreach (var prop in typeof(IParserResult).GetProperties())
            {
                prop.SetValue(this, prop.GetValue(source));
            }
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string TitleHash { get; set; }
        public string Content { get; set; }
        public string ContentHash { get; set; }
        public string Author { get; set; }
        public string DatePublish { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public IEnumerable<string> Images { get; set; }
        public IEnumerable<string> Keywords { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public DateTime UtcCreatedOn { get; set; }
    }
}
