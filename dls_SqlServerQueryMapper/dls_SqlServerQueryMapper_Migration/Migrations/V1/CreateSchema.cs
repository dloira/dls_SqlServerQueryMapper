using FluentMigrator;
using System.Security.Cryptography.Xml;

namespace dls_SqlServerQueryMapper_Migration.Migrations.V1
{
    [Migration(1)]
    public class CreateSchema : Migration
    {
        public override void Up()
        {
            Create.Table("City")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable();

            Create.Table("WeatherForecast")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("TemperatureC").AsInt64().NotNullable()
                .WithColumn("Summary").AsString().NotNullable()
                .WithColumn("CityId").AsInt64().NotNullable();

            Create.ForeignKey()
                .FromTable("WeatherForecast").ForeignColumn("CityId")
                .ToTable("City").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey()
                .FromTable("WeatherForecast").ForeignColumn("CityId")
                .ToTable("City").PrimaryColumn("Id");

            Delete.Table("WeatherForecast");

            Delete.Table("City");
        }
    }
}
