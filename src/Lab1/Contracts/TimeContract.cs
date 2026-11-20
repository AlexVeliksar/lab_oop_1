using Itmo.ObjectOrientedProgramming.Lab1.Models;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public sealed class TimeContract : Contract
{
    public TimeContract(
        AsteroidBelt belt,
        Fleet fleet,
        PriceList priceList,
        decimal duration)
        : base(belt, fleet, priceList)
    {
        if (duration <= 0)
        {
            throw new ArgumentException("Срок контракта должен быть положительным.", nameof(duration));
        }

        Duration = duration;
    }

    public decimal Duration { get; }

    public override bool CanStartTrip(decimal elapsedTime, decimal minedVolume, decimal flightTime)
    {
        return (elapsedTime + flightTime + 1m + flightTime) <= Duration;
    }

    public override bool CanStartCycle(decimal elapsedTime, decimal minedVolume, decimal flightTime)
    {
        return (elapsedTime + 1m + flightTime) <= Duration;
    }

    public override bool HasEnoughTimeForFirstTrip(decimal flightTime)
    {
        return (flightTime + 1m + flightTime) <= Duration;
    }
}
