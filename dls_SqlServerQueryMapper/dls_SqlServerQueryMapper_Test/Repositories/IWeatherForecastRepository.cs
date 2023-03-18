using dls_SqlServerQueryMapper.Attributes.Impl;
using dls_SqlServerQueryMapper.Factory;
using dls_SqlServerQueryMapper.Model.Parameter;
using sqlQueryMapper.Attributes;
using System.Data;

namespace dls_SqlServerQueryMapper_Test.Repositories
{
    public interface IWeatherForecastRepository : IDBDaoFactory
    {
        [DBQuery(QueryName = "weatherForecastQuery")]
        DataTable WeatherForecastQuery(
            DBQueryParameterCollection parameters = null,
            DBQueryWhereConditions whereConditions = null,
            DBQueryPagination paginationConditions = null,
            DataTable dt = null);

        [DBQuery(QueryName = "newWeatherForecastQuery")]
        //[DASource(IsTransactional = true)]
        DataTable NewWeatherForecastQuery(
            DBQueryParameterCollection parameters = null,
            DataTable dt = null);

        [DBQuery(QueryName = "removeWeatherForecastQuery")]
        DataTable RemoveWeatherForecastQuery(
            DBQueryParameterCollection parameters = null,
            DBQueryWhereConditions whereConditions = null,
            DataTable dt = null);

        [DBQuery(QueryName = "changeWeatherForecastQuery")]
        DataTable ChangeWeatherForecastQuery(
            DBQueryParameterCollection parameters = null,
            DBQueryWhereConditions whereConditions = null,
            DataTable dt = null);
    }
}
