using Itmo.ObjectOrientedProgramming.Lab1.Cargo;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Models;
using Itmo.ObjectOrientedProgramming.Lab1.Results;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class ProcessingAndFinanceTests
{
    [Fact]
    public void Processing_RoundsOreUnitsAndLeavesRemainder()
    {
        Mineral pyerite = new("Pyerite");
        Mineral mexallon = new("Mexallon");
        var ore = new Ore(
            "Pyroxeres",
            0.3m,
            [new OreYield(pyerite, 45m), new OreYield(mexallon, 15m)]);

        CompletedContractResult result = RunVolumeContract(ore, 100m, 100m, 0m, 1m);

        Assert.Equal(135m, result.Minerals[pyerite]);
        Assert.Equal(45m, result.Minerals[mexallon]);
        Assert.Equal(33m, result.RemainingOreUnits);
    }

    [Fact]
    public void Processing_KeepsOreRemainderBetweenTrips()
    {
        Mineral mineral = new("Mineral");
        var ore = new Ore("Ore", 1m, [new OreYield(mineral, 10m)]);

        CompletedContractResult result = RunVolumeContract(ore, 60m, 120m, 0m, 1m);

        Assert.Equal(2, result.TripCount);
        Assert.Equal(10m, result.Minerals[mineral]);
        Assert.Equal(20m, result.RemainingOreUnits);
    }

    [Fact]
    public void FullTax_CanMakeProfitNegative()
    {
        Mineral mineral = new("Mineral");
        var ore = new Ore("Ore", 1m, [new OreYield(mineral, 100m)]);

        CompletedContractResult result = RunVolumeContract(ore, 100m, 100m, 1m, 1m);

        Assert.Equal(100m, result.Revenue);
        Assert.Equal(100m, result.Tax);
        Assert.Equal(1m, result.Rent);
        Assert.Equal(-1m, result.Profit);
    }

    [Fact]
    public void TimeContract_ExactlyEqualToMinimalTripTime_IsCompleted()
    {
        Mineral mineral = new("Mineral");
        var ore = new Ore("Ore", 1m, [new OreYield(mineral, 100m)]);
        var belt = new AsteroidBelt("Belt", 1m, ore);
        var ship = new Ship("Ship", 100m, 1m, 1m, new ConstantMiningBehavior(100m));
        var fleet = new Fleet([ship], new SeparateCargoStrategy());
        var prices = new PriceList([new KeyValuePair<Mineral, decimal>(mineral, 1m)]);
        var contract = new TimeContract(belt, fleet, prices, 3m);

        ContractResult calculationResult = new ContractCalculator().Calculate(contract, new Station(0m));

        CompletedContractResult result = Assert.IsType<CompletedContractResult>(calculationResult);
        Assert.Equal(3m, result.ElapsedTime);
        Assert.Equal(100m, result.MinedVolume);
    }

    private static CompletedContractResult RunVolumeContract(
        Ore ore,
        decimal cargoCapacity,
        decimal targetVolume,
        decimal taxRate,
        decimal mineralPrice)
    {
        var belt = new AsteroidBelt("Belt", 0m, ore);
        var ship = new Ship(
            "Ship",
            cargoCapacity,
            1m,
            1m,
            new ConstantMiningBehavior(cargoCapacity));
        var fleet = new Fleet([ship], new SeparateCargoStrategy());
        IEnumerable<KeyValuePair<Mineral, decimal>> prices = ore.Yields
            .Select(item => new KeyValuePair<Mineral, decimal>(item.Mineral, mineralPrice));
        var contract = new VolumeContract(belt, fleet, new PriceList(prices), targetVolume);

        ContractResult calculationResult = new ContractCalculator().Calculate(contract, new Station(taxRate));

        return Assert.IsType<CompletedContractResult>(calculationResult);
    }
}
