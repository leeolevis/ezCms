using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public partial interface IDatabase
    {
        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);
        IMultipleResultReader GetMultiple(string tableName, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);
        IMultipleResultReader GetMultiple(string tableName, string schemaName, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);
        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate = null, int? commandTimeout = null);
        IMultipleResultReader GetMultiple(string tableName, GetMultiplePredicate predicate = null, int? commandTimeout = null);
        IMultipleResultReader GetMultiple(string tableName, string schemaName, GetMultiplePredicate predicate = null, int? commandTimeout = null);
        Task<IMultipleResultReader> GetMultipleAsync(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);
        Task<IMultipleResultReader> GetMultipleAsync(string tableName, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);
        Task<IMultipleResultReader> GetMultipleAsync(string tableName, string schemaName, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);
        Task<IMultipleResultReader> GetMultipleAsync(GetMultiplePredicate predicate = null, int? commandTimeout = null);
        Task<IMultipleResultReader> GetMultipleAsync(string tableName, GetMultiplePredicate predicate = null, int? commandTimeout = null);
        Task<IMultipleResultReader> GetMultipleAsync(string tableName, string schemaName, GetMultiplePredicate predicate = null, int? commandTimeout = null);

    }
    public partial class Database
    {
        public IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null) 
            => GetMultiple(null, predicate, transaction, commandTimeout);
        public IMultipleResultReader GetMultiple(string tableName, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null) 
            => GetMultiple(null, null, predicate, transaction, commandTimeout);
        public IMultipleResultReader GetMultiple(string tableName, string schemaName, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null) 
            => _dapper.GetMultiple(Connection, predicate, transaction, commandTimeout, tableName, schemaName);


        public IMultipleResultReader GetMultiple(GetMultiplePredicate predicate = null, int? commandTimeout = null) 
            => GetMultiple(null, predicate, commandTimeout);
        public IMultipleResultReader GetMultiple(string tableName, GetMultiplePredicate predicate = null, int? commandTimeout = null) 
            => GetMultiple(tableName, null, predicate, commandTimeout);
        public IMultipleResultReader GetMultiple(string tableName, string schemaName, GetMultiplePredicate predicate = null, int? commandTimeout = null)
            => _dapper.GetMultiple(Connection, predicate, _transaction, commandTimeout, tableName, schemaName);


        public async Task<IMultipleResultReader> GetMultipleAsync(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null) 
            => await GetMultipleAsync(null,predicate, transaction, commandTimeout);
        public async Task<IMultipleResultReader> GetMultipleAsync(string tableName, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null) 
            => await GetMultipleAsync(tableName, null, predicate, transaction, commandTimeout);
        public async Task<IMultipleResultReader> GetMultipleAsync(string tableName, string schemaName, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null) 
            => await _dapper.GetMultipleAsync(Connection, predicate, transaction, commandTimeout, tableName, schemaName);


        public async Task<IMultipleResultReader> GetMultipleAsync(GetMultiplePredicate predicate = null, int? commandTimeout = null)
        => await GetMultipleAsync(null, predicate, _transaction, commandTimeout);

        public async Task<IMultipleResultReader> GetMultipleAsync(string tableName, GetMultiplePredicate predicate = null, int? commandTimeout = null)
        => await GetMultipleAsync(tableName,null, predicate, _transaction, commandTimeout);

        public async Task<IMultipleResultReader> GetMultipleAsync(string tableName, string schemaName, GetMultiplePredicate predicate = null, int? commandTimeout = null)
        => await _dapper.GetMultipleAsync(Connection, predicate, _transaction, commandTimeout, tableName, schemaName);

    }
}
