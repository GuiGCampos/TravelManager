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
            var originNode = new NodeModel { Name = origin.ToUpper() };
            var destinationNode = new NodeModel { Name = destination.ToUpper() };

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
