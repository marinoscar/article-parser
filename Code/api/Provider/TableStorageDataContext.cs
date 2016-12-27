using api.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Provider
{
    public class TableStorageDataContext : IDataContext
    {
        CloudStorageAccount _account;
        CloudTableClient _tableClient;

        public TableStorageDataContext()
        {
            _account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["azure.storage.key"]);
            _tableClient = _account.CreateCloudTableClient();
        }

        public T Get<T>(string table, string partitionKey, string rowKey) where T : ITableEntity
        {
            var tableRef = _tableClient.GetTableReference(table);
            var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = tableRef.Execute(operation);
            if (result == null) return default(T);
            return (T)Convert.ChangeType(result.Result, typeof(T));
        }

        public void Insert<T>(string table, IEnumerable<T> entities) where T : ITableEntity
        {
            var tableRef = _tableClient.GetTableReference(table);
            var operation = new TableBatchOperation();
            foreach(var e in entities)
            {
                operation.InsertOrReplace(e);
            }
            tableRef.ExecuteBatch(operation);
        }
    }
}
