using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Xml;
using sqlQueryMapper;
using sqlQueryMapper.Attributes;
using dls_SqlServerQueryMapper.Daos;
using dls_SqlServerQueryMapper.Factory;
using dls_SqlServerQueryMapper.Model.Parameter;
using dls_SqlServerQueryMapper.Model.Connection;

namespace dls_SqlServerQueryMapper.Attributes.Impl
{
    /// <summary>
    /// Attribute for database query execution
    /// </summary>
    public class DBQueryAttribute : Attribute, IDBAttributeAspect
    {
        /// <summary>
        /// Query name to execute
        /// </summary>
        public string QueryName { get; set; }

        /// <inheritdoc/> 
        public object ExecuteAspect(object caller, MethodInfo method, object[] args, IConfiguration conf = null)
        {
            var errorSufix = $"Error on {typeof(DBQueryAttribute).Name}.ExecuteAspect() for method {method?.Name}(): ";

            // Caller has to implement IDBDaoFactory interface
            if (!caller.GetType().GetInterfaces().Contains(typeof(IDBDaoFactory)))
                throw new Exception(errorSufix + $"caller object {caller} doesn't implement IDaoFactory");

            // List of method arguments
            var methodArguments = new List<MethodArgument>();
            var paramsMethod = method.GetParameters().ToList();
            for (int i = 0; i < paramsMethod.Count; i++)
                methodArguments.Add(new MethodArgument(i, paramsMethod[i].ParameterType, args[i]));

            MethodArgument.CheckIsValid(methodArguments);

            // It is not allowed to get 2 arguments of the same type
            var countTypes = methodArguments.Select(ma => ma.ArgType).Distinct().Count();
            if (countTypes != methodArguments.Count)
            {
                throw new Exception($"Method has more than one argument of same type");
            }

            // Assign method arguments to DAParameterCollection, DAWhereConditions, IDatabaseSource and DataTable types
            var parameterCollectionMArgument = methodArguments.Where(ma => typeof(DBQueryParameterCollection).Equals(ma.ArgType)).FirstOrDefault();
            var whereConditionsMArgument = methodArguments.Where(ma => typeof(DBQueryWhereConditions).Equals(ma.ArgType)).FirstOrDefault();
            var paginationConditionsMArgument = methodArguments.Where(ma => typeof(DBQueryPagination).Equals(ma.ArgType)).FirstOrDefault();
            var databaseSourceMArgument = methodArguments.Where(ma => typeof(IDBSource).Equals(ma.ArgType)).FirstOrDefault();
            var dataTableMArgument = methodArguments.Where(ma => typeof(DataTable).Equals(ma.ArgType)).FirstOrDefault();

            // It is mandatory to define an argument for datatable
            if (dataTableMArgument == default)
                throw new Exception(errorSufix + $" Method doesn't have an argument of {typeof(DataTable).Name} type");

            var parameterCollection = (DBQueryParameterCollection)parameterCollectionMArgument?.ArgValue ?? new DBQueryParameterCollection();
            var whereConditions = (DBQueryWhereConditions)whereConditionsMArgument?.ArgValue;
            var paginationConditions = (DBQueryPagination)paginationConditionsMArgument?.ArgValue;
            var databaseSource = (IDBSource)databaseSourceMArgument?.ArgValue;

            string queryString = GetQueryString(QueryName, conf);
            if (queryString == null)
                throw new Exception($"can't find query named '{QueryName}' on XML file");

            // Flag to save if database source was null
            bool databaseSourceWasNull = databaseSource == null;

            try
            {
                // Create database source if needed
                if (databaseSource == null)
                {
                    databaseSource = ((DBDaoFactory)caller).SetDatabaseSource(false);

                    // Replace database source on args to invoke
                    if (databaseSourceMArgument != null)
                        args[databaseSourceMArgument.ArgNumber] = databaseSource;
                }

                IDBDao dao = ((DBDaoFactory)caller).CreateDao();
                DataTable dtResult = dao.Query(queryString, parameterCollection, whereConditions, paginationConditions, QueryName);
                args[dataTableMArgument.ArgNumber] = dtResult;

                // Execute non aspect logic (original method call).
                // Invoke has only to map DataTable result to final result (any type).
                var result = method.Invoke(caller, args);

                // Dispose database source
                if (databaseSourceWasNull)
                    ((DBDaoFactory)caller).ResetDatabaseSource();

                return result;
            }
            catch (TargetInvocationException targetInvocationException)
            {
                Exception innerException = targetInvocationException.InnerException;
                if (innerException != null)
                    ExceptionDispatchInfo.Capture(innerException).Throw();

                throw;
            }
            catch (Exception e)
            {
                string description = $"Exception on {GetType().Name}.ExecuteAspect() method: {e.Message}";
                throw new Exception(description, e);
            }
            finally
            {
                if (databaseSourceWasNull && databaseSource != null)
                    ((DBDaoFactory)caller).ResetDatabaseSource();
            }
        }

        /// <summary>
        /// Returns the SQL string for query name
        /// </summary>
        /// <param name="queryName">query name that identifies query to get</param>
        /// <param name="conf">configuration for settings to read</param>
        /// <returns></returns>
        public static string GetQueryString(string queryName, IConfiguration conf)
        {
            string result = null;

            // Get the path of XML queries file definitions
            Settings settings = new Settings(conf);
            string queryXmlFile = settings.QueryXmlFilePath;

            if (queryXmlFile == null)
                throw new Exception($"variable {settings.QueryXmlFilePath} not found on settings");

            // open the XML file and read it
            using var reader = XmlReader.Create(queryXmlFile, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse });
            while (reader.ReadToFollowing("query"))
            {
                if (reader.GetAttribute("name") == queryName)
                {
                    if (result != null)
                        throw new Exception($"There are more than one query name '{queryName}' on xml file '{queryXmlFile}'. Query name must be unique.");

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.CDATA)
                        {
                            result = reader.Value;
                            break;
                        }

                        if (reader.Name == queryName && reader.NodeType == XmlNodeType.EndElement)
                            throw new Exception($"Can't find CDATA with query string for query name '{queryName}' on xml file '{queryXmlFile}'");
                    }
                }
            }

            return result;
        }
    }
}
