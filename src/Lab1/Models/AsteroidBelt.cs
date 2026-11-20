namespace Itmo.ObjectOrientedProgramming.Lab1.Models;

public sealed class AsteroidBelt
{
    public AsteroidBelt(string name, decimal distance, Ore ore)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название пояса не может быть пустым.", nameof(name));
        }

        if (distance < 0)
        {
            throw new ArgumentException("Расстояние не может быть отрицательным.", nameof(distance));
        }

        Name = name;
        Distance = distance;
        Ore = ore ?? throw new ArgumentNullException(nameof(ore));
    }

    public string Name { get; }

    public decimal Distance { get; }

    public Ore Ore { get; }
}
