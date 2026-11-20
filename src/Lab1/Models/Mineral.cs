namespace Itmo.ObjectOrientedProgramming.Lab1.Models;

public sealed record Mineral
{
    public Mineral(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название минерала не может быть пустым.", nameof(name));
        }

        Name = name;
    }

    public string Name { get; }
}
