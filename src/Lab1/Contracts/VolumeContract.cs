using Itmo.ObjectOrientedProgramming.Lab1.Models;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public sealed class VolumeContract : Contract
{
    public VolumeContract(
        AsteroidBelt belt,
        Fleet fleet,
        PriceList priceList,
        decimal targetVolume)
        : base(belt, fleet, priceList)
    {
        if (targetVolume <= 0)
        {
            throw new ArgumentException("Требуемый объём должен быть положительным.", nameof(targetVolume));
        }

        TargetVolume = targetVolume;
    }

    public decimal TargetVolume { get; }

    public override bool CanStartTrip(decimal elapsedTime, decimal minedVolume, decimal flightTime)
    {
        return minedVolume < TargetVolume;
    }

    public override bool CanStartCycle(decimal elapsedTime, decimal minedVolume, decimal flightTime)
    {
        return minedVolume < TargetVolume;
    }
}
