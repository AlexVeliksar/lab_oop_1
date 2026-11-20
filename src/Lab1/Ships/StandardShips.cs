namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public static class StandardShips
{
    public static Ship CreateVenture()
    {
        return new Ship("Venture", 400m, 4m, 1000m, new ConstantMiningBehavior(100m));
    }

    public static Ship CreateProspect()
    {
        return new Ship("Prospect", 1000m, 5m, 1500m, new ProspectMiningBehavior());
    }

    public static Ship CreateEpithal()
    {
        return new Ship("Epithal", 3000m, 2m, 500m, new ConstantMiningBehavior(0m));
    }
}
