namespace Itmo.ObjectOrientedProgramming.Lab1.Cargo;

public sealed class SharedCargoStrategy : ICargoPlacementStrategy
{
    public bool TryPlace(IReadOnlyList<CargoHold> holds, int miningShipIndex, decimal volume)
    {
        ArgumentNullException.ThrowIfNull(holds);
        if (miningShipIndex < 0 || miningShipIndex >= holds.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(miningShipIndex));
        }

        if (volume < 0)
        {
            throw new ArgumentException("Объём груза не может быть отрицательным.", nameof(volume));
        }

        if (holds.Sum(hold => hold.FreeVolume) < volume)
        {
            return false;
        }

        decimal remainingVolume = volume;
        remainingVolume = LoadAsMuchAsPossible(holds[miningShipIndex], remainingVolume);

        for (int index = 0; index < holds.Count && remainingVolume > 0; index++)
        {
            if (index != miningShipIndex)
            {
                remainingVolume = LoadAsMuchAsPossible(holds[index], remainingVolume);
            }
        }

        return true;
    }

    private static decimal LoadAsMuchAsPossible(CargoHold hold, decimal volume)
    {
        decimal loadedVolume = Math.Min(hold.FreeVolume, volume);
        hold.TryLoad(loadedVolume);
        return volume - loadedVolume;
    }
}
