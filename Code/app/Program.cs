using Microsoft.Azure.Documents.Client;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            var cnnStr =
  @"mongodb://marin:RPvvfDUPL48UBmCHYuoYK5M8TuHTjB3GNzr8cC5OvvOydkJVNmV2DlR1qz1mPPKfDi3S3SubQFUFcNw0wpJ64Q==@marin.documents.azure.com:10250/?ssl=true&sslverifycertificate=false";
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(cnnStr)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);
            var db = client.GetDatabase("kapp");
            db.CreateCollection("contents");
            var collection = db.GetCollection<BsonDocument>("contents");
            collection.InsertOne(new BsonDocument() {
                {"name", "oscar" },
                {"lastname", "marin" }
            });
        }
    }
}
