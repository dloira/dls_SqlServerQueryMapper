using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using dls_SqlServerQueryMapper.Model.Parameter;
using dls_SqlServerQueryMapper.Model.Connection;
using dls_SqlServerQueryMapper.Common.Enum;

namespace dls_SqlServerQueryMapper.Daos.Impl
{
    /// <summary>
    /// Data Access Object to SQl Server
    /// </summary>
    public class DBDaoSqlServer : IDBDao
    {
        /// <summary>
        /// Returns current database source
        /// </summary>
        public IDBSource DatabaseSource { get; }

        /// <summary>
        /// returns logger
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Returns current configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseSource">database source</param>
        /// <param name="logger">logger</param>
        /// <param name="config">configuration</param>
        public DBDaoSqlServer(IDBSource databaseSource, ILogger logger, IConfiguration config = null)
        {
            if (databaseSource == null)
                throw new Exception("Can't create DAO without a databaseSource!!!");

            Logger = logger;
            DatabaseSource = databaseSource;
            Configuration = config;
        }

        /// <summary>
        /// Returns a DataTable object with result of queryString execution
        /// </summary>
        /// <param name="queryString">SQL query string </param>
        /// <param name="parameters">parameter for query</param>
        /// <param name="whereConditions">where conditions</param>
        /// <param name="paginationConditions">pagination conditions</param>
        /// <param name="dataTableName">name to assign to data table result (optional)</param>
        /// <returns></returns>
        public DataTable Query(
            string queryString,
            DBQueryParameterCollection parameters = null,
            DBQueryWhereConditions whereConditions = null,
            DBQueryPagination paginationConditions = null,
            string dataTableName = null)
        {
            SqlCommand sqlCommand = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Avoiding null on parameter collection
                parameters ??= new DBQueryParameterCollection();

                DataTable result = new DataTable(dataTableName);

                string sqlQuery = queryString;
                sqlQuery = this.ReplaceWhereCondition(queryString, whereConditions, parameters.Parameters);
                sqlQuery = ReplacePaginationCondition(sqlQuery, paginationConditions);
                sqlQuery = parameters.PrepareQueryString(sqlQuery);

                var sqlParams = parameters.GetSqlParameters().ToArray();

                sqlCommand = DatabaseSource.CreateCommand();
                sqlCommand.CommandText = sqlQuery;
                sqlCommand.Parameters.AddRange(sqlParams);

                adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(result);

                return result;
            }
            catch (Exception e)
            {
                string description = $"Exception on {GetType().Name}.Query() method: {e.Message}";
                Logger?.LogError(e, description);
                throw new Exception(description, e);
            }
            finally
            {
                if (sqlCommand != null)
                    sqlCommand.Dispose();

                if (adapter != null)
                    adapter.Dispose();
            }
        }

        /// <summary>
        /// Returns a string with original sql string replacing "WHERE_LABEL" with where conditions
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="whereConditions"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string ReplaceWhereCondition(string sqlString, DBQueryWhereConditions whereConditions, ICollection<DBQueryParameter> parameters)
        {
            string WHERE_LABEL = QueryPlaceholder.WHERE_LABEL;

            if (whereConditions == null)
                return sqlString;

            try
            {
                if (!sqlString.Contains(WHERE_LABEL))
                {
                    throw new Exception($"specified SQL doesn't content label '{WHERE_LABEL}' to replace");
                }
                string finalWhere = "";
                if (whereConditions != null)
                {
                    finalWhere = whereConditions.GetSQL(parameters);
                    if (finalWhere != "")
                    {
                        finalWhere = "where " + finalWhere;
                    }
                }

                return sqlString.Replace(WHERE_LABEL, finalWhere);
            }
            catch (Exception e)
            {
                string description = $"Exception on {GetType().Name}.ReplaceWhereCondition() method: {e.Message}";
                Logger?.LogError(e, description);
                throw new Exception(description, e);
            }
        }

        /// <summary>
        /// Returns a string with original sql string replacing "PAGINATION_LABEL" with pagination conditions
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="paginationConditions"></param>
        /// <returns></returns>
        private string ReplacePaginationCondition(string sqlString, DBQueryPagination paginationConditions)
        {
            string PAGINATION_LABEL = QueryPlaceholder.PAGINATION_LABEL;

            if (paginationConditions == null)
                return sqlString;

            try
            {
                if (!sqlString.Contains(PAGINATION_LABEL))
                    throw new Exception($"specified SQL doesn't content label '{PAGINATION_LABEL}' to replace");

                return sqlString.Replace(PAGINATION_LABEL, paginationConditions.GetSQL());
            }
            catch (Exception e)
            {
                string description = $"Exception on {GetType().Name}.ReplacePaginationCondition() method: {e.Message}";
                Logger?.LogError(e, description);
                throw new Exception(description, e);
            }
        }
    }
}
