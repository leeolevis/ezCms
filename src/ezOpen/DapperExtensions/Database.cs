using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Microsoft.Extensions.Options;
using System;
using System.Data;

namespace DapperExtensions
{
    public partial interface IDatabase : IDisposable
    {
        bool HasActiveTransaction { get; }
        IDbConnection Connection { get; }
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        void RunInTransaction(Action action);
        void RunInTransaction(Action<IDatabase> action);
        T RunInTransaction<T>(Func<T> func);
        void ClearCache();
        Guid GetNextGuid();
        String GetNextGuidString();
        IClassMapper GetMap<T>() where T : class;
    }

    public partial class Database : IDatabase
    {

        private readonly IDapperImplementor _dapper;

        private IDbTransaction _transaction;

        public Database(IDbConnection connection, ISqlGenerator sqlGenerator)
        {
            _dapper = new DapperImplementor(sqlGenerator);
            Connection = connection;

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }
        public Database(IOptions<DataBaseOptions> options, IDapperExtensionsConfiguration Configuration)
        {
            _dapper = new DapperImplementor(new SqlGeneratorImpl(Configuration));
            Connection = options.Value.DbConnection();

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public bool HasActiveTransaction => _transaction != null;

        public IDbConnection Connection { get; }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                _transaction?.Rollback();
                Connection.Close();
            }
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = Connection.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        public void RunInTransaction(Action action)
        {
            BeginTransaction();
            try
            {
                action();
                Commit();
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                {
                    Rollback();
                }

                throw ex;
            }
        }

        public void RunInTransaction(Action<IDatabase> action)
        {
            BeginTransaction();
            try
            {
                action(this);
                Commit();
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                {
                    Rollback();
                }

                throw ex;
            }
        }

        public T RunInTransaction<T>(Func<T> func)
        {
            BeginTransaction();
            try
            {
                T result = func();
                Commit();
                return result;
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                {
                    Rollback();
                }

                throw ex;
            }
        }

        public void ClearCache()
        {
            _dapper.SqlGenerator.Configuration.ClearCache();
        }

        public Guid GetNextGuid()
        {
            return _dapper.SqlGenerator.Configuration.GetNextGuid();
        }

        public string GetNextGuidString()
        {
            return _dapper.SqlGenerator.Configuration.GetNextGuid().ToString("N");
        }

        public IClassMapper GetMap<T>() where T : class
        {
            return _dapper.SqlGenerator.Configuration.GetMap<T>();
        }

        
    }

    public class DataBaseOptions
    {
       public ISqlDialect sqlDialect { get; set; }

        public Func<IDbConnection> DbConnection { get; set; }
    }
}
