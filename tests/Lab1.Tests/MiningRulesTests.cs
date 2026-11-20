using Itmo.ObjectOrientedProgramming.Lab1.Cargo;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Models;
using Itmo.ObjectOrientedProgramming.Lab1.Results;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class MiningRulesTests
{
    [Fact]
    public void Prospect_MinesDifferentVolumeInFirstAndNextCycles()
    {
        var behavior = new ProspectMiningBehavior();

        Assert.Equal(75m, behavior.GetMinedVolume(1));
        Assert.Equal(150m, behavior.GetMinedVolume(2));
    }

    [Fact]
    public void SharedCargoStrategy_DoesNotPartiallyLoadCargo()
    {
        IReadOnlyList<CargoHold> holds = [new CargoHold(30m), new CargoHold(30m)];
        var strategy = new SharedCargoStrategy();

        bool placed = strategy.TryPlace(holds, 0, 70m);

        Assert.False(placed);
        Assert.All(holds, hold => Assert.Equal(0m, hold.UsedVolume));
    }

    [Fact]
    public void SharedCargoStrategy_AllowsMinerToUseTransporterHold()
    {
        TestData data = CreateMinerAndTransporterData(new SharedCargoStrategy());

        ContractResult calculationResult = new ContractCalculator().Calculate(data.Contract, new Station(0m));

        CompletedContractResult result = Assert.IsType<CompletedContractResult>(calculationResult);
        Assert.Equal(100m, result.MinedVolume);
    }

    [Fact]
    public void SeparateCargoStrategy_DoesNotAllowMinerToUseTransporterHold()
    {
        TestData data = CreateMinerAndTransporterData(new SeparateCargoStrategy());

        ContractResult calculationResult = new ContractCalculator().Calculate(data.Contract, new Station(0m));

        RejectedContractResult result = Assert.IsType<RejectedContractResult>(calculationResult);
        Assert.Equal(RejectionReason.FleetCannotMine, result.Reason);
    }

    [Fact]
    public void Fleet_UsesSlowestSpeedAndSumsRent()
    {
        var fleet = new Fleet(
            [StandardShips.CreateVenture(), StandardShips.CreateEpithal()],
            new SharedCargoStrategy());

        Assert.Equal(2m, fleet.Speed);
        Assert.Equal(1500m, fleet.HourlyRent);
    }

    [Fact]
    public void FleetWithOnlyTransporter_IsRejected()
    {
        Mineral mineral = new("Mineral");
        var ore = new Ore("Ore", 1m, [new OreYield(mineral, 1m)]);
        var belt = new AsteroidBelt("Belt", 1m, ore);
        var fleet = new Fleet([StandardShips.CreateEpithal()], new SharedCargoStrategy());
        var prices = new PriceList([new KeyValuePair<Mineral, decimal>(mineral, 1m)]);
        var contract = new VolumeContract(belt, fleet, prices, 1m);

        ContractResult calculationResult = new ContractCalculator().Calculate(contract, new Station(0m));

        RejectedContractResult result = Assert.IsType<RejectedContractResult>(calculationResult);
        Assert.Equal(RejectionReason.FleetCannotMine, result.Reason);
    }

    private static TestData CreateMinerAndTransporterData(ICargoPlacementStrategy strategy)
    {
        Mineral mineral = new("Mineral");
        var ore = new Ore("Ore", 1m, [new OreYield(mineral, 100m)]);
        var belt = new AsteroidBelt("Belt", 0m, ore);
        var miner = new Ship("Miner", 50m, 1m, 1m, new ConstantMiningBehavior(100m));
        var transporter = new Ship("Transporter", 50m, 1m, 1m, new ConstantMiningBehavior(0m));
        var fleet = new Fleet([miner, transporter], strategy);
        var prices = new PriceList([new KeyValuePair<Mineral, decimal>(mineral, 1m)]);

        return new TestData(new VolumeContract(belt, fleet, prices, 100m));
    }

    private sealed record TestData(VolumeContract Contract);
}
