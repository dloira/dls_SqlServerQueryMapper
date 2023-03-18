using dls_SqlServerQueryMapper.Daos;
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
    /// Interface for any factory to create DAOs
    /// </summary>
    public interface IDBDaoFactory
    {
        /// <summary>
        /// Build DAO
        /// </summary>
        /// <returns></returns>
        public IDBDao CreateDao();
    }
}
