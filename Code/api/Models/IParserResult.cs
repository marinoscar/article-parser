using System;
using System.Collections.Generic;

namespace api.Models
{
    public interface IParserResult
    {
        string Author { get; set; }
        IEnumerable<string> Categories { get; set; }
        string Content { get; set; }
        string ContentHash { get; set; }
        string DatePublish { get; set; }
        string Id { get; set; }
        IEnumerable<string> Images { get; set; }
        string ImageUrl { get; set; }
        IEnumerable<string> Keywords { get; set; }
        string Title { get; set; }
        string TitleHash { get; set; }
        string Url { get; set; }
        string UserId { get; set; }
        DateTime UtcCreatedOn { get; set; }
    }
}