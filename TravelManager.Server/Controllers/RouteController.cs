using Microsoft.AspNetCore.Mvc;
using System;
using TravelManager.Server.Services;

namespace TravelManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRouteService _routeService;

        public RouteController(ILogger<WeatherForecastController> logger, IRouteService routeService)
        {
            _logger = logger;
            _routeService = routeService;
        }

        [HttpGet(Name = "GetRoute")]
        public IActionResult GetRoute(string from, string to)
        {
            return Ok();
        }

        [HttpPost(Name = "CreateRoute")]
        public async Task<IActionResult> CreateRoute(string from, string to, decimal price)
        {
            await _routeService.CreateRoute(from, to, price);
            return Ok();
        }

        [HttpDelete(Name = "DeleteRoute")]
        public IActionResult DeleteRoute(int routeId)
        {
            return Ok();
        }

        [HttpPatch(Name = "UpdateRoute")]
        public IActionResult UpdateRoute(int routeId, string from, string to, decimal price)
        {
            return Ok();
        }

    }
}
