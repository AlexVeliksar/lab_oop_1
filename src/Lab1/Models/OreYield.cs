namespace Itmo.ObjectOrientedProgramming.Lab1.Models;

public sealed record OreYield
{
    public OreYield(Mineral mineral, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Количество минерала должно быть положительным.", nameof(amount));
        }

        Mineral = mineral ?? throw new ArgumentNullException(nameof(mineral));
        Amount = amount;
    }

    public Mineral Mineral { get; }

    public decimal Amount { get; }
}
