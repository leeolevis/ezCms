using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public partial interface IDatabase 
    {
        IEnumerable<T> GetList<T>(object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetList<T>(string tableName,object predicate, IList<ISort> sort,  IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetList<T>( string tableName, string schemaName,object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;

        IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetList<T>(string tableName, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class;
        IEnumerable<T> GetList<T>(string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class;

        Task<IEnumerable<T>> GetListAsync<T>(object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetListAsync<T>(string tableName,object predicate, IList<ISort> sort,  IDbTransaction transaction, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetListAsync<T>(string tableName, string schemaName,object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        Task<IEnumerable<T>> GetListAsync<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetListAsync<T>(string tableName, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;
        Task<IEnumerable<T>> GetListAsync<T>(string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class;

    }
    public partial class Database
    {

        public IEnumerable<T> GetList<T>(object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class
            => GetList<T>(null, predicate, sort, transaction, commandTimeout, buffered);

        public IEnumerable<T> GetList<T>(string tableName, object predicate, IList<ISort> sort,  IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class
            => GetList<T>(tableName,null, predicate, sort, transaction, commandTimeout, buffered);

        public IEnumerable<T> GetList<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class
            => _dapper.GetList<T>(Connection, predicate, sort, transaction, commandTimeout, buffered, tableName, schemaName);

        public IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class
            => GetList<T>(null, predicate, sort, commandTimeout, buffered);

        public IEnumerable<T> GetList<T>(string tableName, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class
            => GetList<T>(tableName,null, predicate, sort, commandTimeout, buffered);

        public IEnumerable<T> GetList<T>(string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class
            => _dapper.GetList<T>(Connection, predicate, sort, _transaction, commandTimeout, buffered, tableName, schemaName);

        public async Task<IEnumerable<T>> GetListAsync<T>(object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await GetListAsync<T>(null, predicate, sort, transaction, commandTimeout);

        public async Task<IEnumerable<T>> GetListAsync<T>(string tableName, object predicate, IList<ISort> sort,IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await GetListAsync<T>(tableName,null, predicate, sort, transaction, commandTimeout);

        public async Task<IEnumerable<T>> GetListAsync<T>(string tableName, string schemaName, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => await _dapper.GetListAsync<T>(Connection, predicate, sort, transaction, commandTimeout, tableName, schemaName);

        public async Task<IEnumerable<T>> GetListAsync<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => await GetListAsync<T>(null, predicate, sort, commandTimeout);

        public async Task<IEnumerable<T>> GetListAsync<T>(string tableName, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => await GetListAsync<T>(tableName,null, predicate, sort, commandTimeout);

        public async Task<IEnumerable<T>> GetListAsync<T>(string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int? commandTimeout = null) where T : class
            => await _dapper.GetListAsync<T>(Connection, predicate, sort, _transaction, commandTimeout, tableName, schemaName);

    }
}
