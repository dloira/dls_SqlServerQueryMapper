using dls_SqlServerQueryMapper.Attributes.Impl;
using dls_SqlServerQueryMapper.Factory;
using dls_SqlServerQueryMapper.Model.Parameter;
using sqlQueryMapper.Attributes;
using System.Data;

namespace dls_SqlServerQueryMapper_Test.Repositories
{
    public interface ICityRepository : IDBDaoFactory
    {
        [DBQuery(QueryName = "cityQuery")]
        DataTable CityQuery(
            DBQueryParameterCollection parameters = null,
            DBQueryWhereConditions whereConditions = null,
            DataTable dt = null);

        [DBQuery(QueryName = "newCityQuery")]
        // [DASource(IsTransactional = true)]
        DataTable NewCityQuery(
            DBQueryParameterCollection parameters = null,
            DataTable dt = null);
    }
}
