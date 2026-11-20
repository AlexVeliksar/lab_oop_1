namespace Itmo.ObjectOrientedProgramming.Lab1.Models;

public sealed class Station
{
    public Station(decimal taxRate)
    {
        if (taxRate is < 0 or > 1)
        {
            throw new ArgumentException("Ставка налога должна находиться в диапазоне от 0 до 1.", nameof(taxRate));
        }

        TaxRate = taxRate;
    }

    public decimal TaxRate { get; }
}
