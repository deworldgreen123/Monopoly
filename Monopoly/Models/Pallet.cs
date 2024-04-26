namespace Monopoly.Models;

public class Pallet : BaseModel
{
    public DateOnly? ExpirationDate { get; set; }

    public double Volume { get; set; }
}