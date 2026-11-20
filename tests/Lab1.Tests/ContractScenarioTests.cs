using Itmo.ObjectOrientedProgramming.Lab1.Cargo;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Models;
using Itmo.ObjectOrientedProgramming.Lab1.Results;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class ContractScenarioTests
{
    private readonly Mineral _tritanium = new("Tritanium");
    private readonly Mineral _pyerite = new("Pyerite");
    private readonly Mineral _mexallon = new("Mexallon");
    private readonly Mineral _isogen = new("Isogen");
    private readonly ContractCalculator _calculator = new();

    [Fact]
    public void TimeContract_WithSingleVenture_IsCompleted()
    {
        TestData data = CreateDefaultData();
        var contract = new TimeContract(data.Belt, data.Fleet, data.PriceList, 6m);

        ContractResult calculationResult = _calculator.Calculate(contract, new Station(0m));

        CompletedContractResult result = Assert.IsType<CompletedContractResult>(calculationResult);
        Assert.Equal(1, result.TripCount);
        Assert.Equal(6m, result.ElapsedTime);
        Assert.Equal(400m, result.MinedVolume);
        Assert.Equal(8000m, result.Minerals[_tritanium]);
        Assert.Equal(32000m, result.Revenue);
        Assert.Equal(6000m, result.Rent);
        Assert.Equal(26000m, result.Profit);
    }

    [Fact]
    public void Fleet_WithoutShips_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Fleet([], new SharedCargoStrategy()));
    }

    [Fact]
    public void VolumeContract_WithSingleVenture_IsCompleted()
    {
        TestData data = CreateDefaultData();
        var contract = new VolumeContract(data.Belt, data.Fleet, data.PriceList, 1000m);

        ContractResult calculationResult = _calculator.Calculate(contract, new Station(0m));

        CompletedContractResult result = Assert.IsType<CompletedContractResult>(calculationResult);
        Assert.Equal(3, result.TripCount);
        Assert.Equal(16m, result.ElapsedTime);
        Assert.Equal(1000m, result.MinedVolume);
        Assert.Equal(20000m, result.Minerals[_tritanium]);
        Assert.Equal(80000m, result.Revenue);
        Assert.Equal(16000m, result.Rent);
        Assert.Equal(64000m, result.Profit);
    }

    [Fact]
    public void TimeContract_TooShort_IsRejected()
    {
        TestData data = CreateDefaultData();
        var contract = new TimeContract(data.Belt, data.Fleet, data.PriceList, 2m);

        ContractResult calculationResult = _calculator.Calculate(contract, new Station(0m));

        RejectedContractResult result = Assert.IsType<RejectedContractResult>(calculationResult);
        Assert.Equal(RejectionReason.NotEnoughTime, result.Reason);
    }

    [Fact]
    public void Contract_WithIncompletePriceList_IsRejected()
    {
        TestData data = CreateDefaultData();
        var scordite = new Ore(
            "Scordite",
            0.15m,
            [new OreYield(_tritanium, 75m), new OreYield(_pyerite, 55m)]);
        var belt = new AsteroidBelt("Пояс B", 4m, scordite);
        var incompletePriceList = new PriceList([new KeyValuePair<Mineral, decimal>(_tritanium, 4m)]);
        var contract = new TimeContract(belt, data.Fleet, incompletePriceList, 6m);

        ContractResult calculationResult = _calculator.Calculate(contract, new Station(0m));

        RejectedContractResult result = Assert.IsType<RejectedContractResult>(calculationResult);
        Assert.Equal(RejectionReason.MissingMineralPrice, result.Reason);
    }

    private TestData CreateDefaultData()
    {
        var veldspar = new Ore("Veldspar", 0.1m, [new OreYield(_tritanium, 200m)]);
        var belt = new AsteroidBelt("Пояс A", 4m, veldspar);
        var fleet = new Fleet([StandardShips.CreateVenture()], new SharedCargoStrategy());
        var prices = new PriceList(
        [
            new KeyValuePair<Mineral, decimal>(_tritanium, 4m),
            new KeyValuePair<Mineral, decimal>(_pyerite, 10m),
            new KeyValuePair<Mineral, decimal>(_mexallon, 70m),
            new KeyValuePair<Mineral, decimal>(_isogen, 150m),
        ]);

        return new TestData(belt, fleet, prices);
    }

    private sealed record TestData(AsteroidBelt Belt, Fleet Fleet, PriceList PriceList);
}
