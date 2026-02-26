namespace GameInventoryApiStefanKobetich.Models;

// Simple model representing a game in the inventory
public class Game
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
