using dls_SqlServerQueryMapper.Factory.Impl;
using dls_SqlServerQueryMapper.Model.Parameter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sqlQueryMapper.Proxy;
using System;
using System.Data;

namespace dls_SqlServerQueryMapper_Test.Repositories.Impl
{
    public class CityRepository : DBDaoFactorySqlServer, ICityRepository
    {
        public override string ConnectionString { get; }

        public override ILogger Logger { get; }

        public override IConfiguration Configuration { get; }

        //public override object CreateDao(Type daoClass)
        //{
        //    throw new NotImplementedException();
        //}

        private CityRepository(string connectionString, IConfiguration conf = null)
        {
            this.ConnectionString = connectionString;
            this.Configuration = conf;
        }

        public static ICityRepository Create(string? connectionString, IConfiguration conf = null)
        {
            var repository = new CityRepository(connectionString, conf);
            var result = DBProxyFactory<ICityRepository>.Create(repository, conf);
            return result;
        }

        public DataTable CityQuery(DBQueryParameterCollection parameters = null, DBQueryWhereConditions whereConditions = null, DataTable dt = null)
        {
            return dt;
        }

        public DataTable NewCityQuery(DBQueryParameterCollection parameters = null, DataTable dt = null)
        {
            return dt;
        }
    }
}
