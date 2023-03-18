using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using dls_SqlServerQueryMapper_Test.Repositories.Impl;
using dls_SqlServerQueryMapper_Test.Entities;
using sqlQueryMapper.Proxy;
using dls_SqlServerQueryMapper.Model;
using dls_SqlServerQueryMapper.Factory.Impl;
using dls_SqlServerQueryMapper.Model.Connection;
using dls_SqlServerQueryMapper.Model.Parameter;
using dls_SqlServerQueryMapper.Common.Utils;
using dls_SqlServerQueryMapper.Common.Enum;

namespace dls_SqlServerQueryMapper_Test.Services.Impl
{
    public class WeatherForecastService : DBDaoFactorySqlServer, IWeatherForecastService
    {
        public override string ConnectionString { get; }

        public override ILogger Logger { get; }

        public override IConfiguration Configuration { get; }

        private WeatherForecastService(string connectionString, IConfiguration conf = null)
        {
            this.ConnectionString = connectionString;
            this.Configuration = conf;
        }

        public static IWeatherForecastService Create(string? connectionString, IConfiguration conf = null)
        {
            var repository = new WeatherForecastService(connectionString, conf);
            var result = DBProxyFactory<IWeatherForecastService>.Create(repository, conf);
            return result;
        }

        public static IEnumerable<WeatherForecast>? GetWeatherForecastData(string? city, string? summary, int? page, IConfiguration Configuration)
        {
            var repositoryWeatherForecast = WeatherForecastRepository.Create(Configuration.GetConnectionString("sqlserver"), Configuration);

            var parameters = new DBQueryParameterCollection();
            parameters.Add(new DBQueryParameter("topLimit", int.Parse(Configuration["UseCaseDataAccess:TopLimit"])));

            var whereConditions = new DBQueryWhereConditions();
            whereConditions.Operator = LogicOperator.AND;

            if (city != null)
            {
                parameters.Add(new DBQueryParameter("cityName", city));
                whereConditions.ConditionCollection.Add(new DBQueryCondition("CT.Name LIKE @cityName", "cityName"));
            }
            if (summary != null)
            {
                parameters.Add(new DBQueryParameter("summaryText", summary));
                whereConditions.ConditionCollection.Add(new DBQueryCondition("WF.Summary LIKE @summaryText", "summaryText"));
            }

            var paginationConditions = new DBQueryPagination();
            if(page != null)
                paginationConditions.PageNumber = (int)page;
            paginationConditions.PageSize = int.Parse(Configuration["UseCaseDataAccess:PageSize"]);

            var dtable = repositoryWeatherForecast.WeatherForecastQuery(parameters, whereConditions, paginationConditions);

            var weatherForecastList = new List<WeatherForecast>();

            if (dtable != null)
                weatherForecastList = DataMapUtil.ToList<WeatherForecast>(dtable);

            return weatherForecastList;

        }

        public WeatherForecast? AddWeatherForecastData(WeatherForecast weatherForecast, IConfiguration Configuration, IDBSource ds = null)
        {
            var repositoryCity = CityRepository.Create(Configuration.GetConnectionString("sqlserver"), Configuration);
            var repositoryWeatherForecast = WeatherForecastRepository.Create(Configuration.GetConnectionString("sqlserver"), Configuration);

            var parameters = new DBQueryParameterCollection();
            var whereConditions = new DBQueryWhereConditions();

            parameters.Add(new DBQueryParameter("cityName", weatherForecast.City));
            whereConditions.ConditionCollection.Add(new DBQueryCondition("CT.Name LIKE @cityName", "cityName"));

            var dtable = repositoryCity.CityQuery(parameters, whereConditions);

            if (dtable == null || dtable.Rows.Count == 0)
            {
                dtable = repositoryCity.NewCityQuery(parameters);
            }

            parameters.Add(new DBQueryParameter("date", weatherForecast.Date));
            parameters.Add(new DBQueryParameter("temperatureC", weatherForecast.TemperatureC));
            parameters.Add(new DBQueryParameter("summary", weatherForecast.Summary));
            parameters.Add(new DBQueryParameter("cityId", int.Parse(dtable.Rows[0][0].ToString())));

            repositoryWeatherForecast.NewWeatherForecastQuery(parameters);

            return weatherForecast;
        }

        public static bool RemoveWeatherForecastData(int id, IConfiguration Configuration)
        {
            var repositoryWeatherForecast = WeatherForecastRepository.Create(Configuration.GetConnectionString("sqlserver"), Configuration);

            var parameters = new DBQueryParameterCollection();
            var whereConditions = new DBQueryWhereConditions();

            parameters.Add(new DBQueryParameter("id", id));
            whereConditions.ConditionCollection.Add(new DBQueryCondition("Id = @id", "id"));

            repositoryWeatherForecast.RemoveWeatherForecastQuery(parameters, whereConditions);

            return true;
        }

        public static bool ChangeWeatherForecastData(int id, WeatherForecast weatherForecast, IConfiguration Configuration)
        {
            var repositoryCity = CityRepository.Create(Configuration.GetConnectionString("sqlserver"), Configuration);
            var repositoryWeatherForecast = WeatherForecastRepository.Create(Configuration.GetConnectionString("sqlserver"), Configuration);

            var parameters = new DBQueryParameterCollection();
            var whereConditions = new DBQueryWhereConditions();

            parameters.Add(new DBQueryParameter("cityName", weatherForecast.City));
            whereConditions.ConditionCollection.Add(new DBQueryCondition("CT.Name LIKE @cityName", "cityName"));

            var dtable = repositoryCity.CityQuery(parameters, whereConditions);

            if (dtable == null || dtable.Rows.Count == 0)
            {
                repositoryCity.NewCityQuery(parameters);
            }

            parameters = new DBQueryParameterCollection();
            whereConditions = new DBQueryWhereConditions();

            parameters.Add(new DBQueryParameter("dateValue", weatherForecast.Date));
            parameters.Add(new DBQueryParameter("temperatureCValue", weatherForecast.TemperatureC));
            parameters.Add(new DBQueryParameter("summaryText", weatherForecast.Summary));
            parameters.Add(new DBQueryParameter("cityName", weatherForecast.City));
            parameters.Add(new DBQueryParameter("id", id));
            whereConditions.ConditionCollection.Add(new DBQueryCondition("Id = @id", "id"));

            repositoryWeatherForecast.ChangeWeatherForecastQuery(parameters, whereConditions);

            return true;
        }
    }
}
