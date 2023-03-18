using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Model.Connection
{
    /// <summary>
    /// Interface for database access
    /// </summary>
    public interface IDBSource : IDisposable
    {
        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Returns a new SQL Command
        /// </summary>
        /// <returns></returns>
        public SqlCommand CreateCommand();

    }
}
