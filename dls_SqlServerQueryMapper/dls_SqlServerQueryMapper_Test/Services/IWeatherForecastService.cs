using dls_SqlServerQueryMapper.Attributes.Impl;
using dls_SqlServerQueryMapper.Factory;
using dls_SqlServerQueryMapper.Model.Connection;
using dls_SqlServerQueryMapper_Test.Entities;
using Microsoft.Extensions.Configuration;
using sqlQueryMapper.Attributes;
using System.Data;

namespace dls_SqlServerQueryMapper_Test.Services
{
    public interface IWeatherForecastService : IDBDaoFactory
    {
        [DBSource(IsTransactional = true)]
        WeatherForecast? AddWeatherForecastData(WeatherForecast weatherForecast, IConfiguration Configuration, IDBSource ds = null);
    }
}
