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
                throw new ArgumentException("Trecho já existe, atualize o registro");
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
                throw new ArgumentException("Trecho não existe para ser alterado");
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
            origin = ApplyFormat(origin);
            destination = ApplyFormat(destination);
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

        //Implementação Dijkstra
        public async Task<string> GetLowestPrice(string origin, string destination) 
        {
            origin = ApplyFormat(origin);
            destination = ApplyFormat(destination);

            var nodes = await _context.Set<NodeModel>()
                .Include(n => n.OriginRoutes)
                    .ThenInclude(r => r.Destination)
                .Include(n => n.DestinationRoutes)
                    .ThenInclude(r => r.Origin)
                .ToListAsync();

            var originNode = nodes.FirstOrDefault(n => n.Name == origin);
            var destinationNode = nodes.FirstOrDefault(n => n.Name == destination);

            if (originNode == null || destinationNode == null)
            {
                return "Origem ou destino não cadastrado";
            }

            var distances = nodes.ToDictionary(n => n.Id, n => decimal.MaxValue);
            var previousNodes = new Dictionary<int, RouteModel>();
            var unvisited = nodes.Select(n => n.Id).ToList();

            distances[originNode.Id] = 0;

            while (unvisited.Count > 0)
            {
                var currentNodeId = unvisited.OrderBy(id => distances[id]).First();
                unvisited.Remove(currentNodeId);

                if (currentNodeId == destinationNode.Id)
                    break;

                var currentNode = nodes.First(n => n.Id == currentNodeId);
                var routes = currentNode.OriginRoutes;

                foreach (var route in routes)
                {
                    var neighborId = route.DestinationId;
                    if (!unvisited.Contains(neighborId))
                        continue;

                    var newDist = distances[currentNodeId] + route.Price;
                    if (newDist < distances[neighborId])
                    {
                        distances[neighborId] = newDist;
                        previousNodes[neighborId] = route;
                    }
                }
            }

            var path = new List<RouteModel>();
            var currentId = destinationNode.Id;
            while (previousNodes.ContainsKey(currentId))
            {
                var route = previousNodes[currentId];
                path.Insert(0, route);
                currentId = route.OriginId;
            }

            if (path?.Count > 0) 
            {
                decimal total = 0;
                string finalRoute = origin;
                foreach (var route in path)
                {
                    total += route.Price;
                    finalRoute = string.Concat(finalRoute, " - ", route.Destination.Name);
                }
                return $"{finalRoute} ao custo de {total}";
            }

            return "O trecho consultado não existe";
        }
    }
}
