using dls_SqlServerQueryMapper_Test.Entities;
using dls_SqlServerQueryMapper_Test.Repositories.Impl;
using dls_SqlServerQueryMapper_Test.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dls_SqlServerQueryMapper_Test.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _conf;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _conf = configuration;
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecast([FromQuery] string? city, [FromQuery] string? summary, [FromQuery] int? page)
        {
            var response = await Task.FromResult(WeatherForecastService.GetWeatherForecastData(city, summary, page, _conf));
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<WeatherForecast>> PostWeatherForecast([FromBody] WeatherForecast weatherForecast)
        {
            var serviceWeatherForecast = WeatherForecastService.Create(_conf.GetConnectionString("sqlserver"), _conf);
            var response = await Task.FromResult(serviceWeatherForecast.AddWeatherForecastData(weatherForecast, _conf));
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutWeatherForecast([FromQuery] int id, [FromBody] WeatherForecast weatherForecast)
        {
            _ = await Task.FromResult(WeatherForecastService.ChangeWeatherForecastData(id, weatherForecast, _conf));

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteWeatherForecast([FromQuery] int id)
        {
            _ = await Task.FromResult(WeatherForecastService.RemoveWeatherForecastData(id, _conf));
            return NoContent();
        }
    }
}
