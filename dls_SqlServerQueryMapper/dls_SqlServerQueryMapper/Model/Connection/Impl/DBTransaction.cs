using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Model.Connection.Impl
{
    /// <summary>
    /// Database transaction
    /// </summary>
    public class DBTransaction : IDBSource
    {
        private readonly SqlConnection _connection = null;
        private readonly SqlTransaction _transaction = null;
        private bool _isActive = false;
        private bool _disposedValue;

        /// <summary>
        /// Creates a new transaction for a concrete connection string
        /// </summary>
        /// <param name="connectionString">conection string</param>
        /// <param name="isolationLevel">isolation level (default: ReadCommited)</param>
        public DBTransaction(string connectionString, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction(isolationLevel);

            _isActive = true;
        }

        /// <summary>
        /// Returns connection string
        /// </summary>
        public string ConnectionString
        {
            get { return _connection.ConnectionString; }
        }

        /// <summary>
        /// Transaction is active
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
        }

        /// <summary>
        /// Returns a new SQL command
        /// </summary>
        /// <returns></returns>
        public SqlCommand CreateCommand()
        {
            if (!IsActive)
                throw new Exception($"Can't create a command on a {GetType().Name} object when it isn't active. Check that no commit or rollback was executed before");

            var command = _connection.CreateCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            return command;
        }

        /// <summary>
        /// Commit transaction
        /// </summary>
        public void Commit()
        {
            if (!IsActive)
                throw new Exception($"Can't commit on a {GetType().Name} object when it isn't Active. Check that no commit or rollback was executed before");

            _transaction.Commit();
            _isActive = false;

            Dispose();
        }

        /// <summary>
        /// Rollback transaction
        /// </summary>
        public void Rollback()
        {
            if (!IsActive)
                throw new Exception($"Can't rollback on a {GetType().Name} object when it isn't Active. Check that no commit or rollback was executed before");

            _transaction.Rollback();
            _isActive = false;

            Dispose();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            _isActive = false;

            if (!_disposedValue)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                    _connection.Dispose();
                }

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
    }
}
