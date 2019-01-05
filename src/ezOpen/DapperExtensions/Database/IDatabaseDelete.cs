using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public partial interface IDatabase 
    {
        bool Delete<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        bool Delete<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        bool Delete<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        bool Delete<T>(T entity, int? commandTimeout = null) where T : class;
        bool Delete<T>(T entity, string tableName, int? commandTimeout = null) where T : class;
        bool Delete<T>(T entity, string tableName, string schemaName, int? commandTimeout = null) where T : class;
        bool Delete<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        bool Delete<T>(object predicate, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        bool Delete<T>(object predicate, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        bool Delete<T>(object predicate, int? commandTimeout = null) where T : class;
        bool Delete<T>(object predicate, string tableName, int? commandTimeout = null) where T : class;
        bool Delete<T>(object predicate, string tableName, string schemaName, int? commandTimeout = null) where T : class;

        Task<bool> DeleteAsync<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(T entity, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(T entity, string tableName, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(T entity, string tableName, string schemaName, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(object predicate, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(object predicate, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(object predicate, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(object predicate, string tableName, int? commandTimeout = null) where T : class;
        Task<bool> DeleteAsync<T>(object predicate, string tableName, string schemaName, int? commandTimeout = null) where T : class;

    }
    public partial class Database
    {

        public bool Delete<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Delete<T>(entity, null, transaction, commandTimeout);

        public bool Delete<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Delete<T>(entity, tableName, null, transaction, commandTimeout);

        public bool Delete<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class
           => _dapper.Delete(Connection, entity, transaction, commandTimeout, tableName, schemaName);

        public bool Delete<T>(T entity, int? commandTimeout = null) where T : class
            => Delete<T>(entity, string.Empty, commandTimeout);

        public bool Delete<T>(T entity, string tableName, int? commandTimeout = null) where T : class
            => Delete<T>(entity, tableName, string.Empty, commandTimeout);

        public bool Delete<T>(T entity, string tableName, string schemaName, int? commandTimeout = null) where T : class
            => _dapper.Delete(Connection, entity, _transaction, commandTimeout, tableName, schemaName);

        public bool Delete<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Delete<T>(predicate, null, transaction, commandTimeout);

        public bool Delete<T>(object predicate, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Delete<T>(predicate,tableName, null, transaction, commandTimeout);

        public bool Delete<T>(object predicate, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => _dapper.Delete<T>(Connection, predicate, transaction, commandTimeout, tableName, schemaName);

        public bool Delete<T>(object predicate, int? commandTimeout = null) where T : class
            => Delete<T>(predicate, string.Empty, commandTimeout);

        public bool Delete<T>(object predicate, string tableName, int? commandTimeout = null) where T : class
            => Delete<T>(predicate,tableName, string.Empty, commandTimeout);

        public bool Delete<T>(object predicate, string tableName, string schemaName, int? commandTimeout = null) where T : class
            => _dapper.Delete<T>(Connection, predicate, _transaction, commandTimeout, tableName, schemaName);

        public async Task<bool> DeleteAsync<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(entity, null, transaction, commandTimeout);

        public async Task<bool> DeleteAsync<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(entity, tableName, null, transaction, commandTimeout);

        public async Task<bool> DeleteAsync<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await _dapper.DeleteAsync<T>(Connection, entity, transaction, commandTimeout, tableName, schemaName);

        public async Task<bool> DeleteAsync<T>(T entity, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(entity, string.Empty, commandTimeout);

        public async Task<bool> DeleteAsync<T>(T entity, string tableName, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(entity, tableName, string.Empty, commandTimeout);

        public async Task<bool> DeleteAsync<T>(T entity, string tableName, string schemaName, int? commandTimeout = null) where T : class
            => await _dapper.DeleteAsync<T>(Connection, entity, _transaction, commandTimeout, tableName, schemaName);

        public async Task<bool> DeleteAsync<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(predicate, null, transaction, commandTimeout);

        public async Task<bool> DeleteAsync<T>(object predicate, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(predicate, tableName, null, transaction, commandTimeout);

        public async Task<bool> DeleteAsync<T>(object predicate, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await _dapper.DeleteAsync<T>(Connection, predicate, transaction, commandTimeout, tableName, schemaName);

        public async Task<bool> DeleteAsync<T>(object predicate, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(predicate, string.Empty, commandTimeout);

        public async Task<bool> DeleteAsync<T>(object predicate, string tableName, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(predicate, tableName, string.Empty, commandTimeout);

        public async Task<bool> DeleteAsync<T>(object predicate, string tableName, string schemaName, int? commandTimeout = null) where T : class
            => await _dapper.DeleteAsync<T>(Connection, predicate, _transaction, commandTimeout, tableName, schemaName);

    }
}
