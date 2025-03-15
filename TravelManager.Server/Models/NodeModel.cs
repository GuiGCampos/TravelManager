using System.Collections;

public class NodeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<RouteModel> OriginRoutes { get; set; }
    public virtual ICollection<RouteModel> DestinationRoutes { get; set; }
}
