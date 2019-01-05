using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public partial interface IDatabase 
    {

        Page<T> GetPages<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Page<T> GetPages<T>(string tableName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Page<T> GetPages<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Page<T> GetPages<T>(int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;
        Page<T> GetPages<T>(string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;
        Page<T> GetPages<T>(string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;

        Task<Page<T>> GetPagesAsync<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<Page<T>> GetPagesAsync<T>(string tableName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<Page<T>> GetPagesAsync<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<Page<T>> GetPagesAsync<T>(int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;
        Task<Page<T>> GetPagesAsync<T>(string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;
        Task<Page<T>> GetPagesAsync<T>(string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;

    }
    public partial class Database
    {

        public Page<T> GetPages<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => GetPages<T>(null, predicate, sort,page, resultsPerPage, transaction, commandTimeout);

        public Page<T> GetPages<T>(string tableName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => GetPages<T>(tableName, null, predicate, sort, page, resultsPerPage, transaction, commandTimeout);

        public Page<T> GetPages<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class
            =>_dapper.GetPages<T>(Connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, tableName, schemaName);

        public Page<T> GetPages<T>(int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => GetPages<T>(null, page, resultsPerPage, predicate, sort, commandTimeout);

        public Page<T> GetPages<T>(string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => GetPages<T>(tableName, null, page, resultsPerPage, predicate, sort, commandTimeout);

        public Page<T> GetPages<T>(string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => _dapper.GetPages<T>(Connection, predicate, sort, page, resultsPerPage, _transaction, commandTimeout,  tableName, schemaName);

        public async Task<Page<T>> GetPagesAsync<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await GetPagesAsync<T>(null, predicate, sort, page, resultsPerPage, transaction, commandTimeout);

        public async Task<Page<T>> GetPagesAsync<T>(string tableName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await GetPagesAsync<T>(tableName,null, predicate, sort, page, resultsPerPage, transaction, commandTimeout);

        public async Task<Page<T>> GetPagesAsync<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await _dapper.GetPagesAsync<T>(Connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, tableName, schemaName);

        public async Task<Page<T>> GetPagesAsync<T>(int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => await GetPagesAsync<T>(null, page, resultsPerPage, predicate, sort, commandTimeout);

        public async Task<Page<T>> GetPagesAsync<T>(string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => await GetPagesAsync<T>(tableName,null, page, resultsPerPage, predicate, sort, commandTimeout);

        public async Task<Page<T>> GetPagesAsync<T>(string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => await _dapper.GetPagesAsync<T>(Connection, predicate, sort, page, resultsPerPage, _transaction, commandTimeout, tableName, schemaName);

    }
}
