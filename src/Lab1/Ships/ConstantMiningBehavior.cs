namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public sealed class ConstantMiningBehavior : IMiningBehavior
{
    public ConstantMiningBehavior(decimal volumePerCycle)
    {
        if (volumePerCycle < 0)
        {
            throw new ArgumentException("Объём добычи не может быть отрицательным.", nameof(volumePerCycle));
        }

        VolumePerCycle = volumePerCycle;
    }

    public decimal VolumePerCycle { get; }

    public decimal GetMinedVolume(int cycleNumber)
    {
        if (cycleNumber <= 0)
        {
            throw new ArgumentException("Номер цикла должен быть положительным.", nameof(cycleNumber));
        }

        return VolumePerCycle;
    }
}
