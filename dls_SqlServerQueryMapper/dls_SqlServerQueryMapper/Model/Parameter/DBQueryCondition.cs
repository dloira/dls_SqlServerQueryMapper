using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Model.Parameter
{
    /// <summary>
    /// Atomic query condition 
    /// </summary>
    public class DBQueryCondition
    {
        /// <summary>
        /// SQL condition
        /// </summary>
        public string Sql { get; }

        /// <summary>
        /// Parameter name
        /// </summary>
        public string SelectionParameterName { get; }

        /// <summary>
        /// Creates a new data access condition with value parameters 
        /// </summary>
        /// <param name="sql">SQL condition</param>
        /// <param name="selectionParameterName">parameter name that decides if condition must be applied or not (when parameter name is null, it will be applied)</param>
        public DBQueryCondition(string sql, string selectionParameterName = null)
        {
            Sql = sql;
            SelectionParameterName = selectionParameterName;
        }
    }
}
