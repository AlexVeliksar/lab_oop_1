using Itmo.ObjectOrientedProgramming.Lab1.Cargo;
using Itmo.ObjectOrientedProgramming.Lab1.Contracts;
using Itmo.ObjectOrientedProgramming.Lab1.Models;
using Itmo.ObjectOrientedProgramming.Lab1.Ships;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class DomainValidationTests
{
    [Fact]
    public void Station_WithTaxOutsideAllowedRange_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Station(-0.01m));
        Assert.Throws<ArgumentException>(() => new Station(1.01m));
    }

    [Fact]
    public void Station_WithBoundaryTaxRates_CanBeCreated()
    {
        var stationWithoutTax = new Station(0m);
        var stationWithFullTax = new Station(1m);

        Assert.Equal(0m, stationWithoutTax.TaxRate);
        Assert.Equal(1m, stationWithFullTax.TaxRate);
    }

    [Fact]
    public void Ship_WithNonPositiveCargoCapacity_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(
            () => new Ship("Test", 0m, 1m, 1m, new ConstantMiningBehavior(1m)));
    }

    [Fact]
    public void Contract_WithNonPositiveLimit_ThrowsArgumentException()
    {
        Mineral mineral = new("Test mineral");
        var ore = new Ore("Test ore", 1m, [new OreYield(mineral, 1m)]);
        var belt = new AsteroidBelt("Test belt", 0m, ore);
        var fleet = new Fleet([StandardShips.CreateVenture()], new SeparateCargoStrategy());
        var prices = new PriceList([new KeyValuePair<Mineral, decimal>(mineral, 1m)]);

        Assert.Throws<ArgumentException>(() => new TimeContract(belt, fleet, prices, 0m));
        Assert.Throws<ArgumentException>(() => new VolumeContract(belt, fleet, prices, 0m));
    }

    [Fact]
    public void FleetShipCollection_CannotBeChanged()
    {
        var fleet = new Fleet([StandardShips.CreateVenture()], new SeparateCargoStrategy());
        ICollection<Ship> ships = Assert.IsAssignableFrom<ICollection<Ship>>(fleet.Ships);

        Assert.Throws<NotSupportedException>(() => ships.Add(StandardShips.CreateEpithal()));
    }
}
