namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public sealed class ProspectMiningBehavior : IMiningBehavior
{
    public decimal GetMinedVolume(int cycleNumber)
    {
        if (cycleNumber <= 0)
        {
            throw new ArgumentException("Номер цикла должен быть положительным.", nameof(cycleNumber));
        }

        return cycleNumber == 1 ? 75m : 150m;
    }
}
