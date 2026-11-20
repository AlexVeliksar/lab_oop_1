namespace Itmo.ObjectOrientedProgramming.Lab1.Cargo;

public interface ICargoPlacementStrategy
{
    bool TryPlace(IReadOnlyList<CargoHold> holds, int miningShipIndex, decimal volume);
}
