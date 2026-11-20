using System.Collections.ObjectModel;

namespace Itmo.ObjectOrientedProgramming.Lab1.Models;

public sealed class Ore
{
    private readonly ReadOnlyCollection<OreYield> _yields;

    public Ore(string name, decimal unitVolume, IEnumerable<OreYield> yields)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название руды не может быть пустым.", nameof(name));
        }

        if (unitVolume <= 0)
        {
            throw new ArgumentException("Объём единицы руды должен быть положительным.", nameof(unitVolume));
        }

        ArgumentNullException.ThrowIfNull(yields);
        var yieldList = yields.ToList();
        if (yieldList.Count == 0)
        {
            throw new ArgumentException("У руды должен быть хотя бы один минерал в выходе.", nameof(yields));
        }

        if (yieldList.Select(item => item.Mineral).Distinct().Count() != yieldList.Count)
        {
            throw new ArgumentException("Минералы в выходе руды не должны повторяться.", nameof(yields));
        }

        Name = name;
        UnitVolume = unitVolume;
        _yields = yieldList.AsReadOnly();
    }

    public string Name { get; }

    public decimal UnitVolume { get; }

    public IReadOnlyCollection<OreYield> Yields => _yields;
}
