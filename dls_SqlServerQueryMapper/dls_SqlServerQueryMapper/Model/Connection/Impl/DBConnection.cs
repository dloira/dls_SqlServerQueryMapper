using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Model.Connection.Impl
{
    /// <summary>
    /// Database connection
    /// </summary>
    public class DBConnection : IDBSource
    {
        private readonly SqlConnection _connection = null;
        private bool _disposedValue;

        /// <inheritdoc/>
        public string ConnectionString
        {
            get { return _connection.ConnectionString; }
        }

        /// <summary>
        /// Creates a new database connection OPENED
        /// </summary>
        /// <param name="connectionString"></param>
        public DBConnection(string connectionString)
        {
            try
            {
                _connection = new SqlConnection(connectionString);
                _connection.Open();
            }
            catch (Exception e)
            {
                throw new Exception($"Connection to DB with conection string = '{connectionString}' failed", e);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                    _connection.Dispose();

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns a new SQL Command
        /// </summary>
        /// <returns></returns>
        public SqlCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }
    }
}
