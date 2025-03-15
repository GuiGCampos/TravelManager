namespace TravelManager.Server.Services
{
    public interface IRouteService
    {
        Task CreateRoute(string origin, string destination, decimal price);
        Task<(bool, string)> GetRouteDetails(string origin, string destination);
        Task DeleteRoute(string origin, string destination);
        Task UpdateRoute(int routeId, string origin, string destination, decimal price);
    }
}
    