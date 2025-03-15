using Microsoft.AspNetCore.Mvc;
using System;
using TravelManager.Server.Services;

namespace TravelManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet(Name = "GetRoute")]
        public async Task<IActionResult> GetRoute(string from, string to)
        {
            var route = await _routeService.GetRouteDetails(from, to);
            return Ok(route.Item2);
        }

        [HttpPost(Name = "CreateRoute")]
        public async Task<IActionResult> CreateRoute(string from, string to, decimal price)
        {
            await _routeService.CreateRoute(from, to, price);
            return Ok();
        }

        [HttpDelete(Name = "DeleteRoute")]
        public async Task<IActionResult> DeleteRoute(string from, string to)
        {
            await _routeService.DeleteRoute(from, to);
            return Ok();
        }

        [HttpPatch(Name = "UpdateRoute")]
        public async Task<IActionResult> UpdateRoute(int routeId, string from, string to, decimal price)
        {
            await _routeService.UpdateRoute(routeId, from, to, price);
            return Ok();
        }

    }
}
