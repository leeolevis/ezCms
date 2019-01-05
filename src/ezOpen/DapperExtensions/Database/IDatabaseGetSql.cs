using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading.Tasks;
namespace DapperExtensions
{
    public partial interface IDatabase
    {
        T GetSQL<T>(string sql, object dynamicParameters = null, int? commandTimeout = null) where T : class;

        Task<T> GetSQLAsync<T>(string sql, object dynamicParameters = null, int? commandTimeout = null) where T : class;

        IEnumerable<T> GetListSQL<T>(string sql, object dynamicParameters = null, int? commandTimeout = null, bool buffered = true) where T : class;

        Task<IEnumerable<T>> GetListSQLAsync<T>(string sql, object dynamicParameters = null, int? commandTimeout = null) where T : class;

        Page<T> GetPagesSQL<T>(string sql, string sqlCount, int page, int resultsPerPage, object dynamicParameters = null, int? commandTimeout = null, bool buffered = true) where T : class;

        Task<Page<T>> GetPagesSQLAsync<T>(string sql, string sqlCount, int page, int resultsPerPage, object dynamicParameters = null, int? commandTimeout = null) where T : class;

        IEnumerable<T> GetPageSQL<T>(string sql, int page, int resultsPerPage, object dynamicParameters = null, int? commandTimeout = null, bool buffered = true) where T : class;

        Task<IEnumerable<T>> GetPageSQLAsync<T>(string sql, int page, int resultsPerPage, object dynamicParameters = null, int? commandTimeout = null) where T : class;
    }

    public partial class Database
    {
        public T GetSQL<T>(string sql, object dynamicParameters, int? commandTimeout) where T : class
            => _dapper.GetSQL<T>(Connection, sql, dynamicParameters, _transaction, commandTimeout);

        public async Task<T> GetSQLAsync<T>(string sql, object dynamicParameters, int? commandTimeout) where T : class
            => await _dapper.GetSQLAsync<T>(Connection, sql, dynamicParameters, _transaction, commandTimeout);

        public IEnumerable<T> GetListSQL<T>(string sql, object dynamicParameters, int? commandTimeout, bool buffered = true) where T : class
            => _dapper.GetListSQL<T>(Connection, sql, dynamicParameters, _transaction, commandTimeout, buffered);

        public async Task<IEnumerable<T>> GetListSQLAsync<T>(string sql, object dynamicParameters, int? commandTimeout) where T : class
            => await _dapper.GetListSQLAsync<T>(Connection, sql, dynamicParameters, _transaction, commandTimeout);

        public Page<T> GetPagesSQL<T>(string sql, string sqlCount, int page, int resultsPerPage, object dynamicParameters = null, int? commandTimeout = null, bool buffered = true) where T : class
            => _dapper.GetPagesSQL<T>(Connection, sql, sqlCount, dynamicParameters, page, resultsPerPage,_transaction, commandTimeout, buffered);

        public Task<Page<T>> GetPagesSQLAsync<T>(string sql, string sqlCount, int page, int resultsPerPage, object dynamicParameters = null, int? commandTimeout = null) where T : class
            => _dapper.GetPagesSQLAsync<T>(Connection, sql, sqlCount, dynamicParameters, page, resultsPerPage, _transaction, commandTimeout);

        public IEnumerable<T> GetPageSQL<T>(string sql, int page, int resultsPerPage, object dynamicParameters = null, int? commandTimeout = null, bool buffered = true) where T : class
            => _dapper.GetPageSQL<T>(Connection, sql, dynamicParameters, page, resultsPerPage, _transaction, commandTimeout, buffered);

        public async Task<IEnumerable<T>> GetPageSQLAsync<T>(string sql, int page, int resultsPerPage, object dynamicParameters = null, int? commandTimeout = null) where T : class
            => await _dapper.GetPageSQLAsync<T>(Connection, sql, dynamicParameters, page, resultsPerPage, _transaction, commandTimeout);
    }
}
