using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public partial interface IDatabase
    {
        long Count<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        long Count<T>(string tableName, object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        long Count<T>(string tableName, string schemaName, object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        long Count<T>(object predicate = null, int? commandTimeout = null) where T : class;
        long Count<T>(string tableName, object predicate = null, int? commandTimeout = null) where T : class;
        long Count<T>(string tableName, string schemaName, object predicate = null, int? commandTimeout = null) where T : class;
        Task<long> CountAsync<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<long> CountAsync<T>(string tableName, object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<long> CountAsync<T>(string tableName, string schemaName, object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<long> CountAsync<T>(object predicate = null, int? commandTimeout = null) where T : class;
        Task<long> CountAsync<T>(string tableName, object predicate = null, int? commandTimeout = null) where T : class;
        Task<long> CountAsync<T>(string tableName, string schemaName, object predicate = null, int? commandTimeout = null) where T : class;

    }
    public partial class Database 
    {

        public long Count<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Count<T>(null, predicate, transaction, commandTimeout);

        public long Count<T>(string tableName, object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Count<T>(tableName, null, predicate, transaction, commandTimeout);

        public long Count<T>(string tableName, string schemaName, object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => _dapper.Count<T>(Connection, predicate, transaction, commandTimeout, tableName, schemaName);

        public long Count<T>(object predicate = null, int? commandTimeout = null) where T : class
            => Count<T>(null, predicate, commandTimeout);

        public long Count<T>(string tableName, object predicate = null, int? commandTimeout = null) where T : class
            => Count<T>(tableName,null, predicate, commandTimeout);

        public long Count<T>(string tableName, string schemaName, object predicate = null, int? commandTimeout = null) where T : class
            => _dapper.Count<T>(Connection, predicate, _transaction, commandTimeout, tableName, schemaName);

        public async Task<long> CountAsync<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await CountAsync<T>(null, predicate, transaction, commandTimeout);

        public async Task<long> CountAsync<T>(string tableName, object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await CountAsync<T>(tableName, null, predicate, transaction, commandTimeout);

        public async Task<long> CountAsync<T>(string tableName, string schemaName, object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await _dapper.CountAsync<T>(Connection, predicate, transaction, commandTimeout, tableName, schemaName);

        public async Task<long> CountAsync<T>(object predicate = null, int? commandTimeout = null) where T : class
            => await CountAsync<T>(null, predicate, commandTimeout);

        public async Task<long> CountAsync<T>(string tableName, object predicate = null, int? commandTimeout = null) where T : class
            => await CountAsync<T>(tableName, null, predicate, commandTimeout);

        public async Task<long> CountAsync<T>(string tableName, string schemaName, object predicate = null, int? commandTimeout = null) where T : class
            => await _dapper.CountAsync<T>(Connection, predicate, _transaction, commandTimeout, tableName, schemaName);

    }
}
