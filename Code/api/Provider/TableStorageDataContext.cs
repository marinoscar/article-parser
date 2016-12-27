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
    public class TableStorageDataContext
    {
        CloudStorageAccount _account;
        CloudTableClient _tableClient;

        public TableStorageDataContext()
        {
            _account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["azure.storage.key"]);
            _tableClient = _account.CreateCloudTableClient();
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
