using dls_SqlServerQueryMapper.Factory.Impl;
using dls_SqlServerQueryMapper.Model.Parameter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sqlQueryMapper.Proxy;
using System;
using System.Data;

namespace dls_SqlServerQueryMapper_Test.Repositories.Impl
{
    public class WeatherForecastRepository : DBDaoFactorySqlServer, IWeatherForecastRepository
    {
        public override string ConnectionString { get; }

        public override ILogger Logger { get; }

        public override IConfiguration Configuration { get; }

        private WeatherForecastRepository(string connectionString, IConfiguration conf = null)
        {
            this.ConnectionString = connectionString;
            this.Configuration = conf;
        }

        public static IWeatherForecastRepository Create(string? connectionString, IConfiguration conf = null)
        {
            var repository = new WeatherForecastRepository(connectionString, conf);
            var result = DBProxyFactory<IWeatherForecastRepository>.Create(repository, conf);
            return result;
        }

        public DataTable WeatherForecastQuery(DBQueryParameterCollection parameters = null, DBQueryWhereConditions whereConditions = null, DBQueryPagination paginationConditions = null, DataTable dt = null)
        {
            return dt;
        }

        public DataTable NewWeatherForecastQuery(DBQueryParameterCollection parameters = null, DataTable dt = null)
        {
            return dt;
        }

        public DataTable RemoveWeatherForecastQuery(DBQueryParameterCollection parameters = null, DBQueryWhereConditions whereConditions = null, DataTable dt = null)
        {
            return dt;
        }

        public DataTable ChangeWeatherForecastQuery(DBQueryParameterCollection parameters = null, DBQueryWhereConditions whereConditions = null, DataTable dt = null)
        {
            return dt;
        }
    }
}
