using System.Xml.Linq;

public class RouteModel
{
    public int Id { get; set; }
    public int OriginId { get; set; }
    public NodeModel Origin { get; set; }
    public int DestinationId { get; set; }
    public NodeModel Destination { get; set; }
    public decimal Price { get; set; }
}
