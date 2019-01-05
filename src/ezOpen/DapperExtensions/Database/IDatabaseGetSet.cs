using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public partial interface IDatabase
    {
        IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetSet<T>(string tableName, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetSet<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetSet<T>(object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetSet<T>(string tableName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetSet<T>(string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null, bool buffered = true) where T : class;
        Task<IEnumerable<T>> GetSetAsync<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetSetAsync<T>(string tableName, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetSetAsync<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetSetAsync<T>(object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetSetAsync<T>(string tableName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetSetAsync<T>(string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null) where T : class;

    }
    public partial class Database
    {
        public IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class
            => GetSet<T>(null, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);

        public IEnumerable<T> GetSet<T>(string tableName, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class
            => GetSet<T>(tableName, null, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);

        public IEnumerable<T> GetSet<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class
            => _dapper.GetSet<T>(Connection, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered, tableName, schemaName);


        public IEnumerable<T> GetSet<T>(object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null, bool buffered = true) where T : class
            => GetSet<T>(null, predicate, sort, firstResult, maxResults, commandTimeout, buffered);

        public IEnumerable<T> GetSet<T>(string tableName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null, bool buffered = true) where T : class
            => GetSet<T>(tableName, null, predicate, sort, firstResult, maxResults, commandTimeout, buffered);

        public IEnumerable<T> GetSet<T>(string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null, bool buffered = true) where T : class
            => _dapper.GetSet<T>(Connection, predicate, sort, firstResult, maxResults, _transaction, commandTimeout, buffered, tableName, schemaName);


        public async Task<IEnumerable<T>> GetSetAsync<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await GetSetAsync<T>(null, predicate, sort, firstResult, maxResults, transaction, commandTimeout);

        public async Task<IEnumerable<T>> GetSetAsync<T>(string tableName, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await GetSetAsync<T>(tableName, null, predicate, sort, firstResult, maxResults, transaction, commandTimeout);

        public async Task<IEnumerable<T>> GetSetAsync<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await _dapper.GetSetAsync<T>(Connection, predicate, sort, firstResult, maxResults, transaction, commandTimeout, tableName, schemaName);

        public async Task<IEnumerable<T>> GetSetAsync<T>(object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null) where T : class
            => await GetSetAsync<T>(null, predicate, sort, firstResult, maxResults, commandTimeout);

        public async Task<IEnumerable<T>> GetSetAsync<T>(string tableName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null) where T : class
            => await GetSetAsync<T>(tableName, null, predicate, sort, firstResult, maxResults, commandTimeout);

        public async Task<IEnumerable<T>> GetSetAsync<T>(string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, int? commandTimeout = null) where T : class
           => await _dapper.GetSetAsync<T>(Connection, predicate, sort, firstResult, maxResults, _transaction, commandTimeout, tableName, schemaName);

    }
}
