﻿using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace api.Provider
{
    public interface IDataContext
    {
        void Insert<T>(string table, IEnumerable<T> entities) where T : ITableEntity;
    }
}