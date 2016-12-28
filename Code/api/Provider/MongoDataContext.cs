﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace api.Provider
{
    public interface IDataContext
    {
        void Insert<T>(string collection, T entity);
        IEnumerable<T> Select<T>(string collection, FilterDefinition<T> filter);
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

        public void Insert<T>(string collection, T entity)
        {
            var coll = Database.GetCollection<T>(collection);
            coll.InsertOne(entity);
        }

        public IEnumerable<T> Select<T>(string collection, FilterDefinition<T> filter)
        {
            var coll = Database.GetCollection<T>(collection);
            return coll.Find(filter).ToList();
        }


    }    
}