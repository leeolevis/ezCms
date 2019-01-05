using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DapperExtensions
{
    public partial interface IDatabase 
    {

        void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        void Insert<T>(IEnumerable<T> entities, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        void Insert<T>(IEnumerable<T> entities, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        void Insert<T>(IEnumerable<T> entities, int? commandTimeout = null) where T : class;
        void Insert<T>(IEnumerable<T> entities, string tableName, int? commandTimeout = null) where T : class;
        void Insert<T>(IEnumerable<T> entities, string tableName, string schemaName, int? commandTimeout = null) where T : class;

        dynamic Insert<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        dynamic Insert<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        dynamic Insert<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class;
        dynamic Insert<T>(T entity, int? commandTimeout = null) where T : class;
        dynamic Insert<T>(T entity, string tableName, int? commandTimeout = null) where T : class;
        dynamic Insert<T>(T entity, string tableName, string schemaName, int? commandTimeout = null) where T : class;

    }
    public partial class Database
    {

        public void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Insert<T>(entities,null, transaction, commandTimeout);

        public void Insert<T>(IEnumerable<T> entities, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class
           => Insert<T>(entities, tableName, null, transaction, commandTimeout);

        public void Insert<T>(IEnumerable<T> entities, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => _dapper.Insert<T>(Connection, entities, transaction, commandTimeout, tableName, schemaName);

        public void Insert<T>(IEnumerable<T> entities, int? commandTimeout = null) where T : class
            => Insert<T>(entities, string.Empty, commandTimeout);

        public void Insert<T>(IEnumerable<T> entities, string tableName, int? commandTimeout = null) where T : class
            => Insert<T>(entities, tableName, string.Empty, commandTimeout);

        public void Insert<T>(IEnumerable<T> entities, string tableName, string schemaName, int? commandTimeout = null) where T : class
            => _dapper.Insert<T>(Connection, entities, _transaction, commandTimeout, tableName, schemaName);

        public dynamic Insert<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Insert<T>(entity, null, transaction, commandTimeout);

        public dynamic Insert<T>(T entity, string tableName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => Insert<T>(entity, tableName, null, transaction, commandTimeout);

        public dynamic Insert<T>(T entity, string tableName, string schemaName, IDbTransaction transaction, int? commandTimeout = null) where T : class
            => _dapper.Insert<T>(Connection, entity, transaction, commandTimeout, tableName, schemaName);

        public dynamic Insert<T>(T entity, int? commandTimeout = null) where T : class
            => Insert<T>(entity, string.Empty, commandTimeout);

        public dynamic Insert<T>(T entity, string tableName, int? commandTimeout = null) where T : class
            => Insert<T>(entity, string.Empty, string.Empty, commandTimeout);

        public dynamic Insert<T>(T entity, string tableName, string schemaName, int? commandTimeout = null) where T : class
            => _dapper.Insert<T>(Connection, entity, _transaction, commandTimeout, tableName, schemaName);

    }
}
