namespace PcParts.API.Models;

public class Reason
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<ReturnOrder>? ReturnOrders { get; set; }
}