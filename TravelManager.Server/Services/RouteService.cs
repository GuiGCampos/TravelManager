using Microsoft.EntityFrameworkCore;
using System;

namespace TravelManager.Server.Services
{
    public class RouteService : IRouteService
    {
        private readonly DatabaseContext _context;
        public RouteService() 
        {
            _context = new DatabaseContext();
        }
        public async Task CreateRoute(string origin, string destination, decimal price)
        {
            origin = ApplyFormat(origin);
            destination = ApplyFormat(destination);

            var route = await GetRouteDetails(origin, destination);

            if (route.Item1)
            {
                throw new Exception("Trecho já existe, atualize o registro");
            }

            var originNode = await _context.Nodes.FirstOrDefaultAsync(p => p.Name == origin) ?? new NodeModel { Name = origin };

            var destinationNode = await _context.Nodes.FirstOrDefaultAsync(p => p.Name == destination) ?? new NodeModel { Name = destination };

            var newRoute = new RouteModel
            {
                Origin = originNode,
                Destination = destinationNode,
                Price = price
            };

            _context.Routes.Add(newRoute);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoute(int routeId, string origin, string destination, decimal price)
        {
            origin = ApplyFormat(origin);
            destination = ApplyFormat(destination);

            var route = await GetRouteById(routeId);

            if (route is null)
            {
                throw new Exception("Trecho não existe para ser alterado");
            }

            var originNode = await _context.Nodes.FirstOrDefaultAsync(p => p.Name == origin) ?? new NodeModel { Name = origin };
            var destinationNode = await _context.Nodes.FirstOrDefaultAsync(p => p.Name == destination) ?? new NodeModel { Name = destination };
            
            route.Price = price;
            route.Origin = originNode;
            route.Destination = destinationNode;

            _context.Routes.Update(route);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoute(string origin, string destination)
        {
            origin = ApplyFormat(origin);
            destination = ApplyFormat(destination);

            var route = await GetRoute(origin, destination);
            
            if (route != null) 
            {
                await DeleteRouteItem(route);
            }
        }

        private async Task DeleteRouteItem(RouteModel route)
        {
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
        }

        public async Task<(bool, string)> GetRouteDetails(string origin, string destination)
        {
            var route = await GetRoute(origin, destination);

            if (route == null)
            {
                return (false, "Rota não cadastrada");
            }
            else
            {
                return (true, $"Rota {origin} para {destination} custa {route.Price}");
            }
        }

        private async Task<RouteModel?> GetRoute(string origin, string destination)
        {
            return await _context.Routes
                        .Include(r => r.Origin)
                        .Include(r => r.Destination)
                        .FirstOrDefaultAsync(i => i.Origin.Name == origin && i.Destination.Name == destination);
        }

        private async Task<RouteModel?> GetRouteById(int routeId)
        {
            return await _context.Routes
                        .Include(r => r.Origin)
                        .Include(r => r.Destination)
                        .FirstOrDefaultAsync(i => i.Id == routeId);
        }

        private string ApplyFormat(string node)
        {
            return node.Trim().ToUpper();
        }
    }
}
