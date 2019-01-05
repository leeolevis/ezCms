using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public partial interface IDatabase 
    {
        T Get<T>(dynamic id, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        T Get<T>(dynamic id, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        T Get<T>(dynamic id, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        T Get<T>(dynamic id, int? commandTimeout = null) where T : class;
        T Get<T>(dynamic id, string tableName, int? commandTimeout = null) where T : class;
        T Get<T>(dynamic id, string tableName, string schemaName, int? commandTimeout = null) where T : class;

        Task<T> GetAsync<T>(dynamic id, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<T> GetAsync<T>(dynamic id, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<T> GetAsync<T>(dynamic id, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<T> GetAsync<T>(dynamic id, int? commandTimeout = null) where T : class;
        Task<T> GetAsync<T>(dynamic id, string tableName, int? commandTimeout = null) where T : class;
        Task<T> GetAsync<T>(dynamic id, string tableName, string schemaName, int? commandTimeout = null) where T : class;
    }
    public partial class Database
    {

        public T Get<T>(dynamic id, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Get<T>(id, null, transaction, commandTimeout);

        public T Get<T>(dynamic id, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Get<T>(id, tableName, null, transaction, commandTimeout);

        public T Get<T>(dynamic id, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => (T)_dapper.Get<T>(Connection, id, transaction, commandTimeout, tableName, schemaName);

        public T Get<T>(dynamic id, int? commandTimeout = null) where T : class
            => Get<T>(id, string.Empty, commandTimeout);

        public T Get<T>(dynamic id, string tableName, int? commandTimeout = null) where T : class
           => Get<T>(id, tableName, string.Empty, commandTimeout);

        public T Get<T>(dynamic id, string tableName, string schemaName, int? commandTimeout = null) where T : class
            => (T)_dapper.Get<T>(Connection, id, _transaction, commandTimeout, tableName, schemaName);

        public async Task<T> GetAsync<T>(dynamic id, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await GetAsync<T>(id, null, transaction, commandTimeout);

        public async Task<T> GetAsync<T>(dynamic id, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class
             => await GetAsync<T>(id, tableName, null, transaction, commandTimeout);

        public async Task<T> GetAsync<T>(dynamic id, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await _dapper.GetAsync<T>(Connection, id, transaction, commandTimeout, tableName, schemaName);

        public async Task<T> GetAsync<T>(dynamic id, int? commandTimeout = null) where T : class
            => await GetAsync<T>(id, string.Empty, commandTimeout);

        public async Task<T> GetAsync<T>(dynamic id, string tableName, int? commandTimeout = null) where T : class
            => await GetAsync<T>(id, tableName, string.Empty, commandTimeout);

        public async Task<T> GetAsync<T>(dynamic id, string tableName, string schemaName, int? commandTimeout = null) where T : class
            => await _dapper.GetAsync<T>(Connection, id, _transaction, commandTimeout, tableName, schemaName);

    }
}
