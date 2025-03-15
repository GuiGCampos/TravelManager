namespace TravelManager.Server.Services
{
    public interface IRouteService
    {
        Task CreateRoute(string origin, string destination, decimal price);
    }
}
