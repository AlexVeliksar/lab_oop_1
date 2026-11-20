using Itmo.ObjectOrientedProgramming.Lab1.Cargo;
using System.Collections.ObjectModel;

namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public sealed class Fleet
{
    private readonly ReadOnlyCollection<Ship> _ships;

    public Fleet(IEnumerable<Ship> ships, ICargoPlacementStrategy cargoStrategy)
    {
        ArgumentNullException.ThrowIfNull(ships);
        var shipList = ships.ToList();
        if (shipList.Count == 0)
        {
            throw new ArgumentException("Флот не может быть пустым.", nameof(ships));
        }

        _ships = shipList.AsReadOnly();
        CargoStrategy = cargoStrategy ?? throw new ArgumentNullException(nameof(cargoStrategy));
        Speed = shipList.Min(ship => ship.Speed);
        HourlyRent = shipList.Sum(ship => ship.HourlyRent);
    }

    public IReadOnlyList<Ship> Ships => _ships;

    public ICargoPlacementStrategy CargoStrategy { get; }

    public decimal Speed { get; }

    public decimal HourlyRent { get; }
}
