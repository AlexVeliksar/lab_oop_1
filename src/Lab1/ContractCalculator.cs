using Itmo.ObjectOrientedProgramming.Lab1.Cargo;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Models;
using Itmo.ObjectOrientedProgramming.Lab1.Results;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;

namespace Itmo.ObjectOrientedProgramming.Lab1;

public sealed class ContractCalculator
{
    public ContractResult Calculate(Contract contract, Station station)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(station);

        decimal flightTime = contract.Belt.Distance / contract.Fleet.Speed;
        RejectionReason? rejectionReason = Validate(contract, flightTime);
        if (rejectionReason is not null)
        {
            return new RejectedContractResult(rejectionReason.Value);
        }

        decimal elapsedTime = 0;
        decimal minedVolume = 0;
        decimal remainingOreUnits = 0;
        int tripCount = 0;
        var minerals = new Dictionary<Mineral, decimal>();

        while (contract.CanStartTrip(elapsedTime, minedVolume, flightTime))
        {
            elapsedTime += flightTime;
            decimal tripVolume = MineDuringTrip(contract, flightTime, ref elapsedTime, ref minedVolume);
            elapsedTime += flightTime;
            tripCount++;
            ProcessOre(contract.Belt.Ore, tripVolume, minerals, ref remainingOreUnits);
        }

        decimal revenue = CalculateRevenue(contract.PriceList, minerals);
        decimal rent = contract.Fleet.HourlyRent * elapsedTime;
        decimal tax = station.TaxRate * revenue;

        return new CompletedContractResult(
            elapsedTime,
            tripCount,
            minedVolume,
            minerals,
            remainingOreUnits,
            revenue,
            rent,
            tax,
            revenue - rent - tax);
    }

    private static RejectionReason? Validate(Contract contract, decimal flightTime)
    {
        bool hasAllPrices = contract.Belt.Ore.Yields
            .All(oreYield => contract.PriceList.TryGetPrice(oreYield.Mineral, out _));
        if (!hasAllPrices)
        {
            return RejectionReason.MissingMineralPrice;
        }

        if (!CanFleetMineFirstCycle(contract.Fleet))
        {
            return RejectionReason.FleetCannotMine;
        }

        if (!contract.HasEnoughTimeForFirstTrip(flightTime))
        {
            return RejectionReason.NotEnoughTime;
        }

        return null;
    }

    private static bool CanFleetMineFirstCycle(Fleet fleet)
    {
        IReadOnlyList<CargoHold> holds = fleet.Ships
            .Select(ship => new CargoHold(ship.CargoCapacity))
            .ToList();
        bool hasMined = false;

        for (int index = 0; index < fleet.Ships.Count; index++)
        {
            decimal volume = fleet.Ships[index].MiningBehavior.GetMinedVolume(1);
            if (volume > 0 && fleet.CargoStrategy.TryPlace(holds, index, volume))
            {
                hasMined = true;
            }
        }

        return hasMined;
    }

    private static decimal MineDuringTrip(
        Contract contract,
        decimal flightTime,
        ref decimal elapsedTime,
        ref decimal minedVolume)
    {
        IReadOnlyList<CargoHold> holds = contract.Fleet.Ships
            .Select(ship => new CargoHold(ship.CargoCapacity))
            .ToList();
        decimal tripVolume = 0;
        int cycleNumber = 1;

        while (contract.CanStartCycle(elapsedTime, minedVolume, flightTime))
        {
            bool hasMined = false;
            for (int index = 0; index < contract.Fleet.Ships.Count; index++)
            {
                Ship ship = contract.Fleet.Ships[index];
                decimal volume = ship.MiningBehavior.GetMinedVolume(cycleNumber);
                if (volume > 0 && contract.Fleet.CargoStrategy.TryPlace(holds, index, volume))
                {
                    tripVolume += volume;
                    minedVolume += volume;
                    hasMined = true;
                }
            }

            if (!hasMined)
            {
                break;
            }

            elapsedTime += 1m;
            cycleNumber++;
        }

        return tripVolume;
    }

    private static void ProcessOre(
        Ore ore,
        decimal tripVolume,
        Dictionary<Mineral, decimal> minerals,
        ref decimal remainingOreUnits)
    {
        remainingOreUnits += decimal.Truncate(tripVolume / ore.UnitVolume);
        decimal portions = decimal.Truncate(remainingOreUnits / 100m);
        remainingOreUnits -= portions * 100m;

        foreach (OreYield oreYield in ore.Yields)
        {
            minerals.TryGetValue(oreYield.Mineral, out decimal currentAmount);
            minerals[oreYield.Mineral] = currentAmount + (portions * oreYield.Amount);
        }
    }

    private static decimal CalculateRevenue(PriceList priceList, Dictionary<Mineral, decimal> minerals)
    {
        decimal revenue = 0;
        foreach (KeyValuePair<Mineral, decimal> mineral in minerals)
        {
            priceList.TryGetPrice(mineral.Key, out decimal price);
            revenue += mineral.Value * price;
        }

        return revenue;
    }
}
