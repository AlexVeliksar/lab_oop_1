namespace Itmo.ObjectOrientedProgramming.Lab1.Cargo;

public sealed class SeparateCargoStrategy : ICargoPlacementStrategy
{
    public bool TryPlace(IReadOnlyList<CargoHold> holds, int miningShipIndex, decimal volume)
    {
        ArgumentNullException.ThrowIfNull(holds);
        if (miningShipIndex < 0 || miningShipIndex >= holds.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(miningShipIndex));
        }

        return holds[miningShipIndex].TryLoad(volume);
    }
}
