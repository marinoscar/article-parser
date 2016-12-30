using api.core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace api.core.Provider
{
    public interface IDataContext
    {
        void Insert<T>(string collection, T entity);
        IEnumerable<T> Select<T>(string collection, FilterDefinition<T> filter);
        IEnumerable<TResult> Select<TEntity, TResult>(string collection, FilterDefinition<TEntity> filter, ProjectionDefinition<TEntity, TResult> projection);
        void Update<T>(string collection, string id, UpdateDefinition<T> update);
        void Update<T>(string collection, T entity) where T : IIdModel;

        IMongoCollection<T> GetCollection<T>(string name);
    }

    public class MongoDataContext : IDataContext
    {
        public MongoDataContext(string dbName)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(ConfigurationManager.AppSettings["azure.storage.key"] + "/?ssl=true&sslverifycertificate=false"));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);
            Database = client.GetDatabase(dbName);
        }

        public IMongoDatabase Database { get; private set; }


        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }

        public void Insert<T>(string collection, T entity)
        {
            var coll = GetCollection<T>(collection);
            coll.InsertOne(entity);
        }

        public void Update<T>(string collection, T entity) where T : IIdModel
        {
            var coll = GetCollection<T>(collection);
            coll.ReplaceOne(Builders<T>.Filter.Eq("Id", entity.Id), entity);
        }

        public void Update<T>(string collection, string id, UpdateDefinition<T> update)
        {
            var coll = GetCollection<T>(collection);
            coll.UpdateOne(Builders<T>.Filter.Eq("Id", id), update);
        }

        public IEnumerable<T> Select<T>(string collection, FilterDefinition<T> filter)
        {
            var coll = GetCollection<T>(collection);
            return coll.Find(filter).ToList();
        }

        public IEnumerable<TResult> Select<TEntity, TResult>(string collection, FilterDefinition<TEntity> filter, ProjectionDefinition<TEntity, TResult> projection)
        {
            var coll = GetCollection<TEntity>(collection);
            return coll.Find(filter).Project<TResult>(projection).ToList();
        }


    }    
}
