using dls_SqlServerQueryMapper.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper.Daos
{
    /// <summary>
    /// Interface for any DAO type
    /// </summary>
    public interface IDBDao
    {
        public DataTable Query(
            string queryString,
            DBQueryParameterCollection parameters = null,
            DBQueryWhereConditions whereConditions = null,
            DBQueryPagination paginationConditions = null,
            string dataTableName = null);
    }
}
