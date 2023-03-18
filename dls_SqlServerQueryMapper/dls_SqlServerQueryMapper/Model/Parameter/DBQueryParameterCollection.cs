using dls_SqlServerQueryMapper.Common.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Model.Parameter
{
    /// <summary>
    /// Query parameter collection
    /// </summary>
    public class DBQueryParameterCollection
    {
        private ICollection<DBQueryParameter> _parameters = new List<DBQueryParameter>();

        /// <summary>
        /// Data access parameters collection
        /// </summary>
        public ICollection<DBQueryParameter> Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new List<DBQueryParameter>();
                }
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }

        /// <summary>
        /// Adds a new parameter to collection
        /// </summary>
        /// <param name="parameter">parameter to add</param>
        public void Add(DBQueryParameter parameter)
        {
            if (parameter != null)
            {
                if (Parameters.Select(p => p.Name).Contains(parameter.Name))
                    throw new Exception($"Can't add a parameter with name '{parameter.Name}' to collection; it already exists.");

                Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Adds a collection of parameters 
        /// </summary>
        /// <param name="parameters">parameters to add</param>
        public void Add(ICollection<DBQueryParameter> parameters)
        {
            if (parameters != null)
                foreach (var p in parameters)
                    Add(p);
        }

        /// <summary>
        /// Returns a query string ready to use on a command object with sql parameters returned by this.GetSqlParameters() method
        /// </summary>
        /// <param name="queryString">original query string</param>
        /// <returns></returns>
        public string PrepareQueryString(string queryString)
        {
            try
            {   
                // Check parameters with different names
                string duplicateName = Parameters
                    .GroupBy(p => p.Name)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .FirstOrDefault();

                if (duplicateName != null)
                    throw new Exception($"there are at least two parameters named '{duplicateName}' !!");

                // Get the parameters with multiple values 
                var parametersMultiple = Parameters
                    .Where(p => p.HasMultipleValues)
                    .ToList();

                string result = queryString;
                foreach (var p in parametersMultiple)
                {
                    if (p.Values == null || !p.Values.Any())
                    {   
                        // Replace parameter name with "null"
                        result = result.Replace("@" + p.Name, "null");
                    }
                    else
                    {   // Replace parameter name with literal SQL values of parameter (comma separated)
                        var literalSqlValues = p.Values
                            .Select(v => SqlUtil.LiteralSQLValue(v))
                            .ToList();

                        result = result.Replace("@" + p.Name, string.Join(',', literalSqlValues));
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                string description = $"Exception on {GetType().Name}.PrepareQueryString() method: {e.Message}";
                throw new Exception(description, e);
            }
        }


        /// <summary>
        /// Returns a collection of SqlParameters to use in a query
        /// </summary>
        /// <returns></returns>
        public ICollection<SqlParameter> GetSqlParameters()
        {
            ICollection<SqlParameter> result = new List<SqlParameter>();
            foreach (DBQueryParameter p in Parameters)
            {
                if (!p.HasMultipleValues)
                    result.Add(new SqlParameter(p.Name, p.Value));
            }
            return result;
        }
    }
}
