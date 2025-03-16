using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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

        [SwaggerOperation(Summary = "Cria rota de origem e destino")]
        [HttpPost(Name = "CreateRoute")]
        public async Task<IActionResult> CreateRoute(string from, string to, decimal price)
        {
            await _routeService.CreateRoute(from, to, price);
            return Ok();
        }

        [SwaggerOperation(Summary = "Deleta rota de origem e destino")]
        [HttpDelete(Name = "DeleteRoute")]
        public async Task<IActionResult> DeleteRoute(string from, string to)
        {
            await _routeService.DeleteRoute(from, to);
            return Ok();
        }

        [SwaggerOperation(Summary = "Atualiza rota de origem e destino")]
        [HttpPatch(Name = "UpdateRoute")]
        public async Task<IActionResult> UpdateRoute(int routeId, string from, string to, decimal price)
        {
            await _routeService.UpdateRoute(routeId, from, to, price);
            return Ok();
        }

        [SwaggerOperation(Summary = "Consulta rota de origem e destino")]
        [HttpGet(Name = "GetRoute")]
        public async Task<IActionResult> GetRoute(string from, string to)
        {
            var route = await _routeService.GetRouteDetails(from, to);
            return Ok(route.Item2);
        }

        [SwaggerOperation(Summary = "Consulta rota mais barata de origem e destino")]
        [HttpGet("lowest-price", Name = "GetLowestPrice")]
        public async Task<IActionResult> GetLowestPrice(string from, string to)
        {
            var route = await _routeService.GetLowestPrice(from, to);
            return Ok(route);
        }

    }
}
