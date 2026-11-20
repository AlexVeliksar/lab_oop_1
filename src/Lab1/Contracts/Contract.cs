using Itmo.ObjectOrientedProgramming.Lab1.Models;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public abstract class Contract
{
    protected Contract(AsteroidBelt belt, Fleet fleet, PriceList priceList)
    {
        Belt = belt ?? throw new ArgumentNullException(nameof(belt));
        Fleet = fleet ?? throw new ArgumentNullException(nameof(fleet));
        PriceList = priceList ?? throw new ArgumentNullException(nameof(priceList));
    }

    public AsteroidBelt Belt { get; }

    public Fleet Fleet { get; }

    public PriceList PriceList { get; }

    public abstract bool CanStartTrip(decimal elapsedTime, decimal minedVolume, decimal flightTime);

    public abstract bool CanStartCycle(decimal elapsedTime, decimal minedVolume, decimal flightTime);

    public virtual bool HasEnoughTimeForFirstTrip(decimal flightTime)
    {
        return true;
    }
}
