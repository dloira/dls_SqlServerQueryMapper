using dls_SqlServerQueryMapper.Daos.Impl;
using dls_SqlServerQueryMapper.Model.Connection;
using dls_SqlServerQueryMapper.Model.Connection.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Factory.Impl
{
    /// <summary>
    /// Factory to create concrete DAO for SQL Server
    /// </summary>
    public abstract class DBDaoFactorySqlServer : DBDaoFactory
    {
        private IDBSource _databaseSource = null;

        /// <summary>
        /// Connnection string
        /// </summary>
        public abstract string ConnectionString { get; }

        /// <summary>
        /// Logger
        /// </summary>
        public abstract ILogger Logger { get; }

        /// <summary>
        /// Configuration
        /// </summary>
        public abstract IConfiguration Configuration { get; }


        /// <inheritdoc/>
        public override IDBSource GetDatabaseSource()
        {
            return _databaseSource;
        }

        /// <inheritdoc/>
        public override IDBSource SetDatabaseSource(bool transactional)
        {
            if (_databaseSource != null)
                throw new Exception("Can't assign a database source when there is a current assigned");

            try
            {
                if (transactional)
                    _databaseSource = new DBTransaction(ConnectionString);
                else
                    _databaseSource = new DBConnection(ConnectionString);

                return _databaseSource;
            }
            catch (Exception e)
            {
                string description = $"Exception on {GetType().Name}.SetDatabaseSource() method: {e.Message}";
                Logger?.LogError(e, description);
                throw new Exception(description, e);
            }
        }

        /// <inheritdoc/>
        public override void ResetDatabaseSource()
        {
            if (_databaseSource != null)
            {
                _databaseSource.Dispose();
                _databaseSource = null;
            }
        }

        /// <inheritdoc/>
        public override DBDaoSqlServer CreateDao()
        {
            return new DBDaoSqlServer(GetDatabaseSource(), Logger, Configuration);
        }

    }
}
