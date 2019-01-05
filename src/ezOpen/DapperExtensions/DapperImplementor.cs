using Dapper;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
//using Itms.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DapperExtensions
{
    public interface IDapperImplementor
    {
        ISqlGenerator SqlGenerator { get; }

        #region Get
        T Get<T>(IDbConnection connection, dynamic idOrPredicate, bool isPredicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;
        Task<T> GetAsync<T>(IDbConnection connection, dynamic idOrPredicate, bool isPredicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;

        #endregion

        #region Insert
        void Insert<T>(IDbConnection connection, IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class;
        dynamic Insert<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class;

        void InsertBatch<T>(IDbConnection connection, IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class;

        #endregion

        #region Update
        bool Update<T>(IDbConnection connection, T entity, object predicate, bool recordLog, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, bool ignoreAllKeyProperties) where T : class;
        Task<bool> UpdateAsync<T>(IDbConnection connection, T entity, object predicate, bool recordLog, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, bool ignoreAllKeyProperties) where T : class;
        bool UpdateSet<T>(IDbConnection connection, object entity, object predicate, bool recordLog, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class;
        Task<bool> UpdateSetAsync<T>(IDbConnection connection, object entity, object predicate, bool recordLog, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class;

        #endregion

        #region Delete
        bool Delete<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class;
        Task<bool> DeleteAsync<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class;
        bool Delete<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class;
        Task<bool> DeleteAsync<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class;
        #endregion

        #region GetList
        IEnumerable<T> GetList<T>(IDbConnection connection, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout, bool buffered, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;

        Task<IEnumerable<T>> GetListAsync<T>(IDbConnection connection, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;
        #endregion

        #region GetPage
        IEnumerable<T> GetPage<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;
        Task<IEnumerable<T>> GetPageAsync<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;
        #endregion

        #region GetPages
        Page<T> GetPages<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;
        Task<Page<T>> GetPagesAsync<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;
        #endregion

        #region GetSet
        IEnumerable<T> GetSet<T>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;
        Task<IEnumerable<T>> GetSetAsync<T>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class;
        #endregion

        #region Count
        long Count<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join) where T : class;
        Task<long> CountAsync<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join) where T : class;
        #endregion

        #region GetMultiple
        IMultipleResultReader GetMultiple(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName);

        Task<IMultipleResultReader> GetMultipleAsync(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName);
        #endregion

        #region BySQL

        T GetSQLField<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout);

        T GetSQL<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout) where T : class;

        dynamic GetSQLDynamic(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout);

        IEnumerable<dynamic> GetListSQLDynamic(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout, bool buffered);

        Task<T> GetSQLAsync<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout) where T : class;

        IEnumerable<T> GetListSQL<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;

        Task<IEnumerable<T>> GetListSQLAsync<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout) where T : class;

        IEnumerable<T> GetPageSQL<T>(IDbConnection connection, string sql, object dynamicParameters, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;

        Task<IEnumerable<T>> GetPageSQLAsync<T>(IDbConnection connection, string sql, object dynamicParameters, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout) where T : class;

        Page<T> GetPagesSQL<T>(IDbConnection connection, string sql, string sqlCount, object dynamicParameters, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;

        Task<Page<T>> GetPagesSQLAsync<T>(IDbConnection connection, string sql, string sqlCount, object dynamicParameters, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout) where T : class;

        T QuerySQL<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, bool buffered, int? commandTimeout, CommandType commandType = CommandType.Text);

        bool ExecuteSQL(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout, CommandType commandType = CommandType.Text);

        Task<bool> ExecuteSQLAsync(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout, CommandType commandType = CommandType.Text);
        #endregion

        #region RecordSQLLog



        #endregion
    }

    public class DapperImplementor : IDapperImplementor
    {
        public DapperImplementor(ISqlGenerator sqlGenerator)
        {
            SqlGenerator = sqlGenerator;
        }

        public ISqlGenerator SqlGenerator { get; }

        #region Get

        public T Get<T>(IDbConnection connection, dynamic idOrPredicate, bool isPredicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate predicate = isPredicate
                ? GetPredicate(classMap, idOrPredicate)
                : GetIdPredicate(classMap, idOrPredicate);

            var result = GetList<T>(connection, predicate, null, transaction, commandTimeout, true, tableName, schemaName, join, alias).SingleOrDefault();
            return result;
        }
        public async Task<T> GetAsync<T>(IDbConnection connection, dynamic idOrPredicate, bool isPredicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            IPredicate predicate = isPredicate
                ? GetPredicate(classMap, idOrPredicate)
                : GetIdPredicate(classMap, idOrPredicate);
            return (await GetListAsync<T>(connection, predicate, null, transaction, commandTimeout, tableName, schemaName, join, alias)).SingleOrDefault();
        }

        #endregion

        #region Insert
        public void Insert<T>(IDbConnection connection, IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class
        {
            IEnumerable<PropertyInfo> properties = null;
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var notKeyProperties = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            var triggerIdentityColumn = classMap.Properties.SingleOrDefault(p => p.KeyType == KeyType.TriggerIdentity);

            var parameters = new List<DynamicParameters>();
            if (triggerIdentityColumn != null)
            {
                properties = typeof(T).GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.Name != triggerIdentityColumn.PropertyInfo.Name);
            }

            foreach (var e in entities)
            {
                foreach (var column in notKeyProperties)
                {
                    if (column.KeyType == KeyType.Guid && (Guid)column.PropertyInfo.GetValue(e, null) == Guid.Empty)
                    {
                        var comb = SqlGenerator.Configuration.GetNextGuid();
                        column.PropertyInfo.SetValue(e, comb, null);
                    }
                }

                if (triggerIdentityColumn != null)
                {
                    var dynamicParameters = new DynamicParameters();
                    foreach (var prop in properties)
                    {
                        dynamicParameters.Add(prop.Name, prop.GetValue(e, null));
                    }

                    // defaultValue need for identify type of parameter
                    var defaultValue = typeof(T).GetProperty(triggerIdentityColumn.PropertyInfo.Name).GetValue(e, null);
                    dynamicParameters.Add("IdOutParam", direction: ParameterDirection.Output, value: defaultValue);

                    parameters.Add(dynamicParameters);
                }
            }

            var sql = SqlGenerator.Insert(classMap, entities?.FirstOrDefault(), schemaName, tableName);

            if (triggerIdentityColumn == null)
            {
                connection.Execute(sql, entities, transaction, commandTimeout, CommandType.Text);
            }
            else
            {
                connection.Execute(sql, parameters, transaction, commandTimeout, CommandType.Text);
            }
        }

        public dynamic Insert<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName = null) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var nonIdentityKeyProperties = classMap.Properties.Where(p => p.KeyType == KeyType.Guid || p.KeyType == KeyType.Assigned).ToList();
            var identityColumn = classMap.Properties.SingleOrDefault(p => p.KeyType == KeyType.Identity);
            var triggerIdentityColumn = classMap.Properties.SingleOrDefault(p => p.KeyType == KeyType.TriggerIdentity);
            foreach (var column in nonIdentityKeyProperties)
            {
                if (column.KeyType == KeyType.Guid && (Guid)column.PropertyInfo.GetValue(entity, null) == Guid.Empty)
                {
                    var comb = SqlGenerator.Configuration.GetNextGuid();
                    column.PropertyInfo.SetValue(entity, comb, null);
                }
            }

            IDictionary<string, object> keyValues = new ExpandoObject();
            var sql = SqlGenerator.Insert(classMap, entity, schemaName, tableName);
            if (identityColumn != null)
            {
                IEnumerable<long> result;
                if (SqlGenerator.SupportsMultipleStatements())
                {
                    sql += SqlGenerator.Configuration.Dialect.BatchSeperator + SqlGenerator.IdentitySql(classMap, schemaName, tableName);
                    result = connection.Query<long>(sql, entity, transaction, false, commandTimeout, CommandType.Text);
                }
                else
                {
                    connection.Execute(sql, entity, transaction, commandTimeout, CommandType.Text);
                    sql = SqlGenerator.IdentitySql(classMap, schemaName, tableName);
                    result = connection.Query<long>(sql, entity, transaction, false, commandTimeout, CommandType.Text);
                }

                // We are only interested in the first identity, but we are iterating over all resulting items (if any).
                // This makes sure that ADO.NET drivers (like MySql) won't actively terminate the query.
                var hasResult = false;
                var identityInt = 0;
                foreach (var identityValue in result)
                {
                    if (hasResult)
                    {
                        continue;
                    }
                    identityInt = Convert.ToInt32(identityValue);
                    hasResult = true;
                }
                if (!hasResult)
                {
                    throw new InvalidOperationException("The source sequence is empty.");
                }

                keyValues.Add(identityColumn.Name, identityInt);
                identityColumn.PropertyInfo.SetValue(entity, identityInt, null);
            }
            else if (triggerIdentityColumn != null)
            {
                var dynamicParameters = new DynamicParameters();
                foreach (var prop in entity.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.Name != triggerIdentityColumn.PropertyInfo.Name))
                {
                    dynamicParameters.Add(prop.Name, prop.GetValue(entity, null));
                }

                // defaultValue need for identify type of parameter
                var defaultValue = entity.GetType().GetProperty(triggerIdentityColumn.PropertyInfo.Name).GetValue(entity, null);
                dynamicParameters.Add("IdOutParam", direction: ParameterDirection.Output, value: defaultValue);

                connection.Execute(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text);

                var value = dynamicParameters.Get<object>(SqlGenerator.Configuration.Dialect.ParameterPrefix + "IdOutParam");
                keyValues.Add(triggerIdentityColumn.Name, value);
                triggerIdentityColumn.PropertyInfo.SetValue(entity, value, null);
            }
            else
            {
                connection.Execute(sql, entity, transaction, commandTimeout, CommandType.Text);
            }

            foreach (var column in nonIdentityKeyProperties)
            {
                keyValues.Add(column.Name, column.PropertyInfo.GetValue(entity, null));
            }

            if (keyValues.Count == 1)
            {
                return keyValues.First().Value;
            }

            return keyValues;
        }

        //批量插入
        [Obsolete("这个版本只针对SQLServer")]
        public void InsertBatch<T>(IDbConnection connection, IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class
        {
            var tblName = string.Format("dbo.{0}", typeof(T).Name);
            var tran = (SqlTransaction)transaction;
            using (var bulkCopy = new SqlBulkCopy(connection as SqlConnection, SqlBulkCopyOptions.TableLock, tran))
            {
                bulkCopy.BatchSize = entityList.Count();
                bulkCopy.DestinationTableName = tblName;
                var table = new DataTable();
                ISqlGenerator sqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration());
                var classMap = sqlGenerator.Configuration.GetMap<T>();
                var props = classMap.Properties.Where(x => x.Ignored == false).ToArray();
                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyInfo.PropertyType) ?? propertyInfo.PropertyInfo.PropertyType);
                }
                var values = new object[props.Count()];
                foreach (var itemm in entityList)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].PropertyInfo.GetValue(itemm, null);
                    }
                    table.Rows.Add(values);
                }
                bulkCopy.WriteToServer(table);
            }
        }

        #endregion

        #region Update

        public bool Update<T>(IDbConnection connection, T entity, object predicate, bool recordLog, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName = null, bool ignoreAllKeyProperties = false) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var wherePredicate = predicate == null ? GetKeyPredicate(classMap, entity) : GetPredicate(classMap, predicate);
            var parameters = new Dictionary<string, object>();
            var sql = SqlGenerator.Update(classMap, entity, wherePredicate, parameters, schemaName, tableName);
            var dynamicParameters = new DynamicParameters();

            var columns = ignoreAllKeyProperties
                ? classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly) && p.KeyType == KeyType.NotAKey)
                : classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity || p.KeyType == KeyType.Assigned));

            foreach (var property in ReflectionHelper.GetObjectValues(entity).Where(property => columns.Any(c => c.Name == property.Key)))
            {
                dynamicParameters.Add(property.Key, property.Value);
            }

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return connection.Execute(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }
        public async Task<bool> UpdateAsync<T>(IDbConnection connection, T entity, object predicate, bool recordLog, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, bool ignoreAllKeyProperties) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var wherePredicate = predicate == null ? GetKeyPredicate(classMap, entity) : GetPredicate(classMap, predicate);
            var parameters = new Dictionary<string, object>();
            var sql = SqlGenerator.Update(classMap, entity, wherePredicate, parameters, schemaName, tableName);
            var dynamicParameters = new DynamicParameters();

            var columns = ignoreAllKeyProperties
                ? classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly) && p.KeyType == KeyType.NotAKey)
                : classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity || p.KeyType == KeyType.Assigned));

            foreach (var property in ReflectionHelper.GetObjectValues(entity).Where(property => columns.Any(c => c.Name == property.Key)))
            {
                dynamicParameters.Add(property.Key, property.Value);
            }

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return await connection.ExecuteAsync(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }

        public bool UpdateSet<T>(IDbConnection connection, object entity, object predicate, bool recordLog, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName = null) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var wherePredicate = predicate == null ? GetSetKeyPredicate<T>(classMap, entity) : GetPredicate(classMap, predicate);
            var parameters = new Dictionary<string, object>();
            var sql = SqlGenerator.UpdateSet(classMap, entity, wherePredicate, parameters, schemaName, tableName);
            var dynamicParameters = new DynamicParameters();

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity || p.KeyType == KeyType.Assigned));

            foreach (var property in ReflectionHelper.GetObjectValues(entity).Where(property => columns.Any(c => c.Name.Equals(property.Key))))
            {
                dynamicParameters.Add(property.Key, property.Value);
            }

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return connection.Execute(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }
        public async Task<bool> UpdateSetAsync<T>(IDbConnection connection, object entity, object predicate, bool recordLog, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var wherePredicate = predicate == null ? GetSetKeyPredicate<T>(classMap, entity) : GetPredicate(classMap, predicate);
            var parameters = new Dictionary<string, object>();
            var sql = SqlGenerator.UpdateSet(classMap, entity, wherePredicate, parameters, schemaName, tableName);
            var dynamicParameters = new DynamicParameters();

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity || p.KeyType == KeyType.Assigned));

            foreach (var property in ReflectionHelper.GetObjectValues(entity).Where(property => columns.Any(c => c.Name.Equals(property.Key))))
            {
                dynamicParameters.Add(property.Key, property.Value);
            }

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return await connection.ExecuteAsync(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }
        #endregion

        #region Delete
        public bool Delete<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName = null) where T : class
        {
            var build = BuildDelete(entity, null, tableName, schemaName);
            return connection.Execute(build.sql, build.dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }
        public bool Delete<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class
        {
            var build = BuildDelete<T>(null, predicate, tableName, schemaName);
            return connection.Execute(build.sql, build.dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }

        public async Task<bool> DeleteAsync<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class
        {
            var build = BuildDelete(entity, null, tableName, schemaName);
            return await connection.ExecuteAsync(build.sql, build.dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }


        public async Task<bool> DeleteAsync<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName) where T : class
        {
            var build = BuildDelete<T>(null, predicate, tableName, schemaName);
            return await connection.ExecuteAsync(build.sql, build.dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }


        protected (string sql, DynamicParameters dynamicParameters) BuildDelete<T>(T entity, object predicate, string tableName, string schemaName) where T : class
        {
            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var wherePredicate = entity == null && predicate != null ? GetPredicate(classMap, predicate) : GetKeyPredicate(classMap, entity);

            var parameters = new Dictionary<string, object>();
            var sql = SqlGenerator.Delete(classMap, wherePredicate, parameters, schemaName, tableName);
            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            return (sql, dynamicParameters);
        }


        #endregion

        #region GetList
        public IEnumerable<T> GetList<T>(IDbConnection connection, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout, bool buffered, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var build = BuildList<T>(predicate, sort, tableName, schemaName, join, alias);
            return connection.Query<T>(build.sql, build.dynamicParameters, transaction, buffered, commandTimeout, CommandType.Text);
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(IDbConnection connection, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var build = BuildList<T>(predicate, sort, tableName, schemaName, join, alias);
            return await connection.QueryAsync<T>(build.sql, build.dynamicParameters, transaction, commandTimeout, CommandType.Text);
        }

        protected (string sql, DynamicParameters dynamicParameters) BuildList<T>(object predicate, IList<ISort> sort, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            VerifyJoinPredicate(join, predicate);

            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var parameters = new Dictionary<string, object>();
            var wherePredicate = GetPredicate(classMap, predicate);
            var sql = SqlGenerator.Select(classMap, wherePredicate, sort, parameters, schemaName, tableName, join, alias);


            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            return (sql, dynamicParameters);
        }


        #endregion

        #region GetPage
        public IEnumerable<T> GetPage<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var build = BuildPage<T>(predicate, sort, page, resultsPerPage, tableName, schemaName, join, alias);
            return connection.Query<T>(build.sql, build.dynamicParameters, transaction, buffered, commandTimeout, CommandType.Text);
        }

        public async Task<IEnumerable<T>> GetPageAsync<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var build = BuildPage<T>(predicate, sort, page, resultsPerPage, tableName, schemaName, join, alias);
            return await connection.QueryAsync<T>(build.sql, build.dynamicParameters, transaction, commandTimeout, CommandType.Text);
        }

        protected (string sql, DynamicParameters dynamicParameters) BuildPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            VerifyJoinPredicate(join, predicate);

            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var wherePredicate = GetPredicate(classMap, predicate);
            var parameters = new Dictionary<string, object>();

            var sql = SqlGenerator.SelectPaged(classMap, wherePredicate, sort, page, resultsPerPage, parameters, schemaName, tableName, join, alias);
            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            return (sql, dynamicParameters);
        }


        #endregion

        #region GetPages

        public Page<T> GetPages<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var PageResult = new Page<T>
            {
                CurrentPage = page,
                ItemsPerPage = resultsPerPage,
                TotalItems = Count<T>(connection, predicate, transaction, commandTimeout, tableName, schemaName, @join)
            };
            if (PageResult.TotalItems == 0)
            {
                PageResult.Items = new List<T>();
                return PageResult;
            }
            var build = BuildPage<T>(predicate, sort, page, resultsPerPage, tableName, schemaName, join, alias);
            PageResult.Items = connection.Query<T>(build.sql, build.dynamicParameters, transaction, false, commandTimeout, CommandType.Text);
            return PageResult;
        }
        public async Task<Page<T>> GetPagesAsync<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var PageResult = new Page<T>
            {
                CurrentPage = page,
                ItemsPerPage = resultsPerPage,
                TotalItems = await CountAsync<T>(connection, predicate, transaction, commandTimeout, tableName,
                    schemaName, join)
            };
            if (PageResult.TotalItems == 0)
            {
                PageResult.Items = new List<T>();
                return PageResult;
            }
            var build = BuildPage<T>(predicate, sort, page, resultsPerPage, tableName, schemaName, join, alias);

            PageResult.Items = await connection.QueryAsync<T>(build.sql, build.dynamicParameters, transaction, commandTimeout, CommandType.Text);

            return PageResult;
        }
        #endregion

        #region GetSet
        public IEnumerable<T> GetSet<T>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var build = BuildPage<T>(predicate, sort, firstResult, maxResults, tableName, schemaName, join, alias);
            return connection.Query<T>(build.sql, build.dynamicParameters, transaction, buffered, commandTimeout, CommandType.Text);
        }

        public async Task<IEnumerable<T>> GetSetAsync<T>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            var build = BuildPage<T>(predicate, sort, firstResult, maxResults, tableName, schemaName, join, alias);
            return await connection.QueryAsync<T>(build.sql, build.dynamicParameters, transaction, commandTimeout, CommandType.Text);
        }

        protected (string sql, DynamicParameters dynamicParameters) BuildGetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, string tableName, string schemaName, IList<IJoinPredicate> join, IList<IJoinAliasPredicate> alias) where T : class
        {
            VerifyJoinPredicate(join, predicate);

            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var wherePredicate = GetPredicate(classMap, predicate);
            var parameters = new Dictionary<string, object>();
            var sql = SqlGenerator.SelectSet(classMap, wherePredicate, sort, firstResult, maxResults, parameters, schemaName, tableName, join, alias);
            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            return (sql, dynamicParameters);
        }

        #endregion

        #region Count
        public long Count<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join) where T : class
        {
            var build = BuildCount<T>(predicate, tableName, schemaName, join);
            return (long)(connection.Query(build.sql, build.dynamicParameters, transaction, false, commandTimeout, CommandType.Text).Single().Total);
        }

        public async Task<long> CountAsync<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName, IList<IJoinPredicate> join) where T : class
        {
            var build = BuildCount<T>(predicate, tableName, schemaName, join);
            return (int)(await connection.QueryAsync(build.sql, build.dynamicParameters, transaction, commandTimeout, CommandType.Text)).Single().Total;
        }

        protected (string sql, DynamicParameters dynamicParameters) BuildCount<T>(object predicate, string tableName, string schemaName, IList<IJoinPredicate> join) where T : class
        {
            VerifyJoinPredicate(join, predicate);

            var classMap = SqlGenerator.Configuration.GetMap<T>();
            var wherePredicate = GetPredicate(classMap, predicate);
            var parameters = new Dictionary<string, object>();
            var sql = SqlGenerator.Count(classMap, wherePredicate, parameters, schemaName, tableName, join);
            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }
            return (sql, dynamicParameters);
        }

        #endregion

        #region GetMultiple

        public IMultipleResultReader GetMultiple(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName)
        {
            if (SqlGenerator.SupportsMultipleStatements())
            {
                return GetMultipleByBatch(connection, predicate, transaction, commandTimeout, tableName, schemaName);
            }

            return GetMultipleBySequence(connection, predicate, transaction, commandTimeout, tableName, schemaName);
        }
        public async Task<IMultipleResultReader> GetMultipleAsync(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName)
        {
            if (SqlGenerator.SupportsMultipleStatements())
            {
                return await GetMultipleByBatchAsync(connection, predicate, transaction, commandTimeout, tableName, schemaName);
            }

            return await GetMultipleBySequenceAsync(connection, predicate, transaction, commandTimeout, tableName, schemaName);
        }

        #endregion

        #region BySQL

        //查询单个字段扩展 TODO:
        public T GetSQLField<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout)
        {
            var value = connection.ExecuteScalar<T>(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text);
            return value;
        }

        public T GetSQL<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            var result = GetSQLQuery<T>(connection, sql, dynamicParameters, transaction, commandTimeout, false).SingleOrDefault();
            return result;
        }

        public dynamic GetSQLDynamic(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout)
        {
            var result = GetSQLQuery<dynamic>(connection, sql, dynamicParameters, transaction, commandTimeout, false).SingleOrDefault();
            return result;
        }

        public IEnumerable<dynamic> GetListSQLDynamic(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout, bool buffered)
        {
            var result = GetSQLQuery<dynamic>(connection, sql, dynamicParameters, transaction, commandTimeout, buffered);
            return result;
        }


        public async Task<T> GetSQLAsync<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            var result = (await GetSQLQueryAsync<T>(connection, sql, dynamicParameters, transaction, commandTimeout)).SingleOrDefault();
            return result;
        }

        public IEnumerable<T> GetListSQL<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            var result = GetSQLQuery<T>(connection, sql, dynamicParameters, transaction, commandTimeout, buffered);
            return result;
        }

        public async Task<IEnumerable<T>> GetListSQLAsync<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            var result = await GetSQLQueryAsync<T>(connection, sql, dynamicParameters, transaction, commandTimeout);
            return result;
        }

        public IEnumerable<T> GetPageSQL<T>(IDbConnection connection, string sql, object dynamicParameters, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            sql = SqlGenerator.GetPagingSql(sql, page, resultsPerPage, null);
            return connection.Query<T>(sql, dynamicParameters, transaction, buffered, commandTimeout, CommandType.Text);
        }


        public async Task<IEnumerable<T>> GetPageSQLAsync<T>(IDbConnection connection, string sql, object dynamicParameters, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            sql = SqlGenerator.GetPagingSql(sql, page, resultsPerPage, null);
            return await connection.QueryAsync<T>(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text);
        }


        public Page<T> GetPagesSQL<T>(IDbConnection connection, string sql, string sqlCount, object dynamicParameters, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            var PageResult = new Page<T>
            {
                CurrentPage = page,
                ItemsPerPage = resultsPerPage,
                TotalItems = (long)connection.ExecuteScalar(sqlCount, dynamicParameters, transaction, commandTimeout,
                    CommandType.Text)
            };
            sql = SqlGenerator.GetPagingSql(sql, page, resultsPerPage, null);
            PageResult.Items = connection.Query<T>(sql, dynamicParameters, transaction, buffered, commandTimeout, CommandType.Text);
            return PageResult;
        }

        public async Task<Page<T>> GetPagesSQLAsync<T>(IDbConnection connection, string sql, string sqlCount, object dynamicParameters, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            var PageResult = new Page<T>
            {
                CurrentPage = page,
                ItemsPerPage = resultsPerPage,
                TotalItems = (long)(await connection.ExecuteScalarAsync(sqlCount, dynamicParameters, transaction,
                    commandTimeout, CommandType.Text))
            };
            sql = SqlGenerator.GetPagingSql(sql, page, resultsPerPage, null);
            PageResult.Items = await connection.QueryAsync<T>(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text);

            return PageResult;
        }

        public T QuerySQL<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, bool buffered, int? commandTimeout, CommandType commandType = CommandType.Text)
        {
            var result = connection.Query<T>(sql, dynamicParameters, transaction, buffered, commandTimeout, commandType).FirstOrDefault();
            return result;
        }

        public bool ExecuteSQL(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout, CommandType commandType = CommandType.Text)
        {
            var result = connection.Execute(sql, dynamicParameters, transaction, commandTimeout, commandType) > 0;
            return result;
        }

        public async Task<bool> ExecuteSQLAsync(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout, CommandType commandType = CommandType.Text)
        {
            var result = await connection.ExecuteAsync(sql, dynamicParameters, transaction, commandTimeout, commandType) > 0;
            return result;
        }

        #endregion

        #region Helpers

        //TODO 记录SQL更新
        public void RecordSqlLog(IDbConnection connection, object entity, object dynamicParameters)
        {

        }

        //TODO 临时
        protected IEnumerable<T> GetSQLQuery<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            return connection.Query<T>(sql, dynamicParameters, transaction, buffered, commandTimeout, CommandType.Text);
        }

        protected async Task<IEnumerable<T>> GetSQLQueryAsync<T>(IDbConnection connection, string sql, object dynamicParameters, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            return await connection.QueryAsync<T>(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text);
        }

        protected IPredicate GetPredicate(IClassMapper classMap, object predicate)
        {
            var wherePredicate = predicate as IPredicate;
            if (wherePredicate == null && predicate != null)
            {
                wherePredicate = GetEntityPredicate(classMap, predicate);
            }

            return wherePredicate;
        }

        protected IPredicate GetIdPredicate(IClassMapper classMap, object id)
        {
            var isSimpleType = ReflectionHelper.IsSimpleType(id.GetType());
            var keys = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            IDictionary<string, object> paramValues = null;
            IList<IPredicate> predicates = new List<IPredicate>();
            if (!isSimpleType)
            {
                paramValues = ReflectionHelper.GetObjectValues(id);
            }

            foreach (var key in keys)
            {
                var value = id;
                if (!isSimpleType)
                {
                    value = paramValues[key.Name];
                }

                var predicateType = typeof(FieldPredicate<>).MakeGenericType(classMap.EntityType);

                var fieldPredicate = Activator.CreateInstance(predicateType) as IFieldPredicate;
                fieldPredicate.Not = false;
                fieldPredicate.Operator = Operator.Eq;
                fieldPredicate.PropertyName = key.Name;
                fieldPredicate.Value = value;
                predicates.Add(fieldPredicate);
            }

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }

        protected IPredicate GetKeyPredicate<T>(IClassMapper classMap, T entity) where T : class
        {
            var whereFields = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            if (!whereFields.Any())
            {
                throw new ArgumentException("At least one Key column must be defined.");
            }

            IList<IPredicate> predicates = (from field in whereFields
                                            select new FieldPredicate<T>
                                            {
                                                Not = false,
                                                Operator = Operator.Eq,
                                                PropertyName = field.Name,
                                                Value = field.PropertyInfo.GetValue(entity, null)
                                            }).Cast<IPredicate>().ToList();

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }

        protected IPredicate GetSetKeyPredicate<T>(IClassMapper classMap, object entity) where T : class
        {
            var whereFields = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            if (!whereFields.Any())
            {
                throw new ArgumentException("At least one Key column must be defined.");
            }
            var vKeyValue = ReflectionHelper.GetObjectValues(entity);
            IList<IPredicate> predicates = (from field in whereFields
                                            select new FieldPredicate<T>
                                            {
                                                Not = false,
                                                Operator = Operator.Eq,
                                                PropertyName = field.Name,
                                                Value = vKeyValue.First(w => w.Key.Equals(field.Name, StringComparison.OrdinalIgnoreCase)).Value
                                            }).Cast<IPredicate>().ToList();

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }

        protected IPredicate GetEntityPredicate(IClassMapper classMap, object entity)
        {
            var predicateType = typeof(FieldPredicate<>).MakeGenericType(classMap.EntityType);
            IList<IPredicate> predicates = new List<IPredicate>();
            foreach (var kvp in ReflectionHelper.GetObjectValues(entity))
            {
                var fieldPredicate = Activator.CreateInstance(predicateType) as IFieldPredicate;
                fieldPredicate.Not = false;
                fieldPredicate.Operator = Operator.Eq;
                fieldPredicate.PropertyName = kvp.Key;
                fieldPredicate.Value = kvp.Value;
                predicates.Add(fieldPredicate);
            }

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }

        protected GridReaderResultReader GetMultipleByBatch(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName)
        {
            var parameters = new Dictionary<string, object>();
            var sql = new StringBuilder();
            foreach (var item in predicate.Items)
            {
                var classMap = SqlGenerator.Configuration.GetMap(item.Type);
                var itemPredicate = item.Value as IPredicate;
                if (itemPredicate == null && item.Value != null)
                {
                    itemPredicate = GetPredicate(classMap, item.Value);
                }

                sql.AppendLine(SqlGenerator.Select(classMap, itemPredicate, item.Sort, parameters, schemaName, tableName) + SqlGenerator.Configuration.Dialect.BatchSeperator);
            }

            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var grid = connection.QueryMultiple(sql.ToString(), dynamicParameters, transaction, commandTimeout, CommandType.Text);
            return new GridReaderResultReader(grid);
        }

        protected SequenceReaderResultReader GetMultipleBySequence(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName)
        {
            IList<SqlMapper.GridReader> items = new List<SqlMapper.GridReader>();
            foreach (var item in predicate.Items)
            {
                var parameters = new Dictionary<string, object>();
                var classMap = SqlGenerator.Configuration.GetMap(item.Type);
                var itemPredicate = item.Value as IPredicate;
                if (itemPredicate == null && item.Value != null)
                {
                    itemPredicate = GetPredicate(classMap, item.Value);
                }

                var sql = SqlGenerator.Select(classMap, itemPredicate, item.Sort, parameters, schemaName, tableName);
                var dynamicParameters = new DynamicParameters();
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }

                var queryResult = connection.QueryMultiple(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text);
                items.Add(queryResult);
            }

            return new SequenceReaderResultReader(items);
        }


        protected async Task<GridReaderResultReader> GetMultipleByBatchAsync(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName)
        {
            var parameters = new Dictionary<string, object>();
            var sql = new StringBuilder();
            foreach (var item in predicate.Items)
            {
                var classMap = SqlGenerator.Configuration.GetMap(item.Type);
                var itemPredicate = item.Value as IPredicate;
                if (itemPredicate == null && item.Value != null)
                {
                    itemPredicate = GetPredicate(classMap, item.Value);
                }

                sql.AppendLine(SqlGenerator.Select(classMap, itemPredicate, item.Sort, parameters, schemaName, tableName) + SqlGenerator.Configuration.Dialect.BatchSeperator);
            }

            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var grid = await connection.QueryMultipleAsync(sql.ToString(), dynamicParameters, transaction, commandTimeout, CommandType.Text);
            return new GridReaderResultReader(grid);
        }

        protected async Task<SequenceReaderResultReader> GetMultipleBySequenceAsync(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout, string tableName, string schemaName)
        {
            IList<SqlMapper.GridReader> items = new List<SqlMapper.GridReader>();
            foreach (var item in predicate.Items)
            {
                var parameters = new Dictionary<string, object>();
                var classMap = SqlGenerator.Configuration.GetMap(item.Type);
                var itemPredicate = item.Value as IPredicate;
                if (itemPredicate == null && item.Value != null)
                {
                    itemPredicate = GetPredicate(classMap, item.Value);
                }

                var sql = SqlGenerator.Select(classMap, itemPredicate, item.Sort, parameters, schemaName, tableName);
                var dynamicParameters = new DynamicParameters();
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }

                var queryResult = await connection.QueryMultipleAsync(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text);
                items.Add(queryResult);
            }

            return new SequenceReaderResultReader(items);
        }

        /// <summary>
        /// 检测join模式下的条件参数类型
        /// </summary>
        /// <param name="join"></param>
        /// <param name="predicate"></param>
        protected void VerifyJoinPredicate(IList<IJoinPredicate> join, object predicate)
        {
            //联合查询时，参数必须是IPredicate格式，不能是anonymoustype、IEnumerable<KeyValuePair<TKey, TValue>>
            if (join != null && join.Count > 0 && predicate != null && (predicate as IPredicate) == null)
            {
                throw new Exception(" join predicate = IPredicate");
            }
        }
        #endregion
    }
}
