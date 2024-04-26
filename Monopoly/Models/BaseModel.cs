namespace Monopoly.Models;

public abstract class BaseModel
{
    
    public Guid Id { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public double Weight { get; set; }

    public double SelfVolume()
    {
        return Width * Height * Depth;
    }
}