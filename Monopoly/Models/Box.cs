using System.ComponentModel.DataAnnotations;

namespace Monopoly.Models;

public class Box : BaseModel
{
    public Guid PalletId { get; set; }
    public DateOnly? ProductionDate { get; set; }
    public DateOnly? ExpirationDate { get; set; }
}