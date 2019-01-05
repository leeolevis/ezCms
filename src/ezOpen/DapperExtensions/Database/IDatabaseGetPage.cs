using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public partial interface IDatabase 
    {

        IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetPage<T>(string tableName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetPage<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetPage<T>(int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetPage<T>(string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetPage<T>(string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class;

        Task<IEnumerable<T>> GetPageAsync<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetPageAsync<T>(string tableName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetPageAsync<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetPageAsync<T>(int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetPageAsync<T>(string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetPageAsync<T>(string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;

    }
    public partial class Database
    {

        public IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class
            => GetPage<T>(null, predicate, sort, page, resultsPerPage, transaction, commandTimeout);

        public IEnumerable<T> GetPage<T>(string tableName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class
            => GetPage<T>(tableName,null, predicate, sort, page, resultsPerPage, transaction, commandTimeout);

        public IEnumerable<T> GetPage<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class
            => _dapper.GetPage<T>(Connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered, tableName, schemaName);

        public IEnumerable<T> GetPage<T>(int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class
            => GetPage<T>(null, page, resultsPerPage, predicate, sort, commandTimeout);

        public IEnumerable<T> GetPage<T>(string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class
            => GetPage<T>(tableName,null, page, resultsPerPage, predicate, sort, commandTimeout);

        public IEnumerable<T> GetPage<T>(string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class
            => _dapper.GetPage<T>(Connection, predicate, sort, page, resultsPerPage, _transaction, commandTimeout, buffered, tableName, schemaName);

        public async Task<IEnumerable<T>> GetPageAsync<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await GetPageAsync<T>(null, predicate, sort, page, resultsPerPage, transaction, commandTimeout);

        public async Task<IEnumerable<T>> GetPageAsync<T>(string tableName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await GetPageAsync<T>(tableName,null, predicate, sort, page, resultsPerPage, transaction, commandTimeout);

        public async Task<IEnumerable<T>> GetPageAsync<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await _dapper.GetPageAsync<T>(Connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, tableName, schemaName);

        public async Task<IEnumerable<T>> GetPageAsync<T>(int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => await GetPageAsync<T>(null, page, resultsPerPage,predicate, sort, commandTimeout);

        public async Task<IEnumerable<T>> GetPageAsync<T>(string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => await GetPageAsync<T>(tableName,null, page, resultsPerPage, predicate, sort, commandTimeout);

        public async Task<IEnumerable<T>> GetPageAsync<T>(string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => await _dapper.GetPageAsync<T>(Connection, predicate, sort, page, resultsPerPage, _transaction, commandTimeout, tableName, schemaName);

    }
}
