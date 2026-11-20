namespace Itmo.ObjectOrientedProgramming.Lab1.Cargo;

public sealed class CargoHold
{
    public CargoHold(decimal capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Вместимость трюма должна быть положительной.", nameof(capacity));
        }

        Capacity = capacity;
    }

    public decimal Capacity { get; }

    public decimal UsedVolume { get; private set; }

    public decimal FreeVolume => Capacity - UsedVolume;

    public bool TryLoad(decimal volume)
    {
        if (volume < 0)
        {
            throw new ArgumentException("Объём груза не может быть отрицательным.", nameof(volume));
        }

        if (volume > FreeVolume)
        {
            return false;
        }

        UsedVolume += volume;
        return true;
    }
}
