using Microsoft.EntityFrameworkCore;

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
            origin = origin.ToUpper();
            destination = destination.ToUpper();
            var routes = await _context.Routes
                .Include(r => r.Origin)
                .Include(r => r.Destination)
                .ToListAsync();

            if (routes.Any(i => i.Origin.Name == origin && i.Destination.Name == destination))
            {
                throw new Exception("Destino já existe, atualize o registro");
            }

            var originNode = new NodeModel { Name = origin };
            var destinationNode = new NodeModel { Name = destination };

            var route = new RouteModel
            {
                Origin = originNode,
                Destination = destinationNode,
                Price = price
            };

            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
        }
    }
}
