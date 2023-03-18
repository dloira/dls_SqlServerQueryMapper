using dls_SqlServerQueryMapper.Daos;
using dls_SqlServerQueryMapper.Model.Connection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Factory
{
    /// <summary>
    /// Factyory method creator
    /// </summary>
    public abstract class DBDaoFactory : IDBDaoFactory
    {
        /// <summary>
        /// Logger
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Returns the current database source 
        /// </summary>
        /// <returns></returns>
        public abstract IDBSource GetDatabaseSource();

        /// <summary>
        /// Assigns a new current database source and returns it
        /// </summary>
        /// <param name="transactional">if database source is transactional or not</param>
        public abstract IDBSource SetDatabaseSource(bool transactional);

        /// <summary>
        /// Reset the current database source, disponsing and setting it to null
        /// </summary>
        public abstract void ResetDatabaseSource();

        /// <summary>
        /// Returns a DAO base object (not associated to model and key classes) for current database source
        /// </summary>
        /// <returns></returns>
        public abstract IDBDao CreateDao();

    }
}
