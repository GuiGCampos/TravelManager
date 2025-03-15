using System.Collections;

public class NodeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<RouteModel> OriginRoutes { get; set; }
    public IList<RouteModel> DestinationRoutes { get; set; }
}
