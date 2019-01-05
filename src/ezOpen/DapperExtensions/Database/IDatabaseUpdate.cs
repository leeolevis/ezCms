using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public partial interface IDatabase 
    {
        bool Update<T>(T entity, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        bool Update<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        bool Update<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;

        bool Update<T>(T entity, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        bool Update<T>(T entity, string tableName, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        bool Update<T>(T entity, string tableName, string schemaName, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;

        bool Update<T>(T entity, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        bool Update<T>(T entity, string tableName, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        bool Update<T>(T entity, string tableName, string schemaName, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;

        bool Update<T>(T entity, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        bool Update<T>(T entity, string tableName, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        bool Update<T>(T entity, string tableName, string schemaName, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;


        Task<bool> UpdateAsync<T>(T entity, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        Task<bool> UpdateAsync<T>(T entity, string tableName, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        Task<bool> UpdateAsync<T>(T entity, string tableName, string schemaName, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;

        Task<bool> UpdateAsync<T>(T entity, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        Task<bool> UpdateAsync<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        Task<bool> UpdateAsync<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;


        Task<bool> UpdateAsync<T>(T entity, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        Task<bool> UpdateAsync<T>(T entity, string tableName, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        Task<bool> UpdateAsync<T>(T entity, string tableName, string schemaName, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;

        Task<bool> UpdateAsync<T>(T entity, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        Task<bool> UpdateAsync<T>(T entity, string tableName, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;
        Task<bool> UpdateAsync<T>(T entity, string tableName, string schemaName, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class;

    }
    public partial class Database
    {

        public bool Update<T>(T entity, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(entity, null, transaction, commandTimeout, ignoreAllKeyProperties);

        public bool Update<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(entity, tableName, null, transaction, commandTimeout, ignoreAllKeyProperties);

        public bool Update<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => _dapper.Update<T>(Connection, entity, transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);

        public bool Update<T>(T entity, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
           => Update<T>(entity,  null, predicate,transaction, commandTimeout, ignoreAllKeyProperties);

        public bool Update<T>(T entity, string tableName, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(entity, tableName, null, predicate, transaction, commandTimeout, ignoreAllKeyProperties);

        public bool Update<T>(T entity, string tableName, string schemaName, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => _dapper.Update<T>(Connection, entity,predicate, transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);


        public bool Update<T>(T entity, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(entity, string.Empty, commandTimeout, ignoreAllKeyProperties);

        public bool Update<T>(T entity, string tableName, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(entity, tableName, string.Empty, commandTimeout, ignoreAllKeyProperties);

        public bool Update<T>(T entity, string tableName, string schemaName, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => _dapper.Update<T>(Connection, entity, _transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);


        public bool Update<T>(T entity, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(entity, string.Empty,predicate, commandTimeout, ignoreAllKeyProperties);

        public bool Update<T>(T entity, string tableName, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(entity, tableName, string.Empty, predicate, commandTimeout, ignoreAllKeyProperties);

        public bool Update<T>(T entity, string tableName, string schemaName, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => _dapper.Update<T>(Connection, entity, predicate, _transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);



      

        public async Task<bool> UpdateAsync<T>(T entity, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(entity, null, transaction, commandTimeout, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(entity, tableName, null, transaction, commandTimeout, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await _dapper.UpdateAsync<T>(Connection, entity, transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
          => await UpdateAsync<T>(entity, null, predicate, transaction, commandTimeout, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, string tableName, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(entity, tableName, null, predicate, transaction, commandTimeout, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, string tableName, string schemaName, object predicate, IDbTransaction transaction, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await _dapper.UpdateAsync<T>(Connection, entity, predicate, transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);


        public async Task<bool> UpdateAsync<T>(T entity, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(entity, string.Empty, commandTimeout, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, string tableName, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(entity, tableName, string.Empty, commandTimeout, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, string tableName, string schemaName, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await _dapper.UpdateAsync<T>(Connection, entity, _transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(entity, string.Empty, predicate, commandTimeout, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, string tableName, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(entity, tableName, string.Empty, predicate, commandTimeout, ignoreAllKeyProperties);

        public async Task<bool> UpdateAsync<T>(T entity, string tableName, string schemaName, object predicate, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await _dapper.UpdateAsync<T>(Connection, entity, predicate, _transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);

    }
}
