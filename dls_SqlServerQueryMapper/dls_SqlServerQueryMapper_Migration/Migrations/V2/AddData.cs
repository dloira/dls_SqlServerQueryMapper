using FluentMigrator;

namespace dls_SqlServerQueryMapper_Migration.Migrations.V2
{
    [Migration(2)]
    public class AddData : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("City").Row(new { Name = "New York" });
            Insert.IntoTable("City").Row(new { Name = "Tokyo" });
            Insert.IntoTable("City").Row(new { Name = "Berlin" });
            Insert.IntoTable("City").Row(new { Name = "Buenos Aires" });

            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = -37, Summary = "Freezing", CityId = 1 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = -20, Summary = "Bracing", CityId = 2 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = -15, Summary = "Chilly", CityId = 3 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 0, Summary = "Cool", CityId = 4 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 14, Summary = "Mild", CityId = 1 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 20, Summary = "Warm", CityId = 2 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 27, Summary = "Balmy", CityId = 3 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 32, Summary = "Hot", CityId = 4 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 39, Summary = "Sweltering", CityId = 1 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 42, Summary = "Scorching", CityId = 2 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 25, Summary = "Balmy", CityId = 4 });
            Insert.IntoTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = -13, Summary = "Chilly", CityId = 4 });
        }

        public override void Down()
        {
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = -37, Summary = "Freezing", CityId = 1 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = -20, Summary = "Bracing", CityId = 2 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = -15, Summary = "Chilly", CityId = 3 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 0, Summary = "Cool", CityId = 4 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 14, Summary = "Mild", CityId = 1 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 20, Summary = "Warm", CityId = 2 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 27, Summary = "Balmy", CityId = 3 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 32, Summary = "Hot", CityId = 4 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 39, Summary = "Sweltering", CityId = 1 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 42, Summary = "Scorching", CityId = 2 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = 25, Summary = "Balmy", CityId = 4 });
            Delete.FromTable("WeatherForecast").Row(new { Date = "2023-01-01 12:00:00", TemperatureC = -13, Summary = "Chilly", CityId = 4 });

            Delete.FromTable("City").Row(new { Name = "New York" });
            Delete.FromTable("City").Row(new { Name = "Tokyo" });
            Delete.FromTable("City").Row(new { Name = "Berlin" });
            Delete.FromTable("City").Row(new { Name = "Buenos Aires" });
        }
    }
}
