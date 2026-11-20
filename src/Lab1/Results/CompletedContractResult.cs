using Itmo.ObjectOrientedProgramming.Lab1.Models;
using System.Collections.ObjectModel;

namespace Itmo.ObjectOrientedProgramming.Lab1.Results;

public sealed class CompletedContractResult : ContractResult
{
    private readonly ReadOnlyDictionary<Mineral, decimal> _minerals;

    public CompletedContractResult(
        decimal elapsedTime,
        int tripCount,
        decimal minedVolume,
        IDictionary<Mineral, decimal> minerals,
        decimal remainingOreUnits,
        decimal revenue,
        decimal rent,
        decimal tax,
        decimal profit)
    {
        ElapsedTime = elapsedTime;
        TripCount = tripCount;
        MinedVolume = minedVolume;
        _minerals = new ReadOnlyDictionary<Mineral, decimal>(
            new Dictionary<Mineral, decimal>(minerals));
        RemainingOreUnits = remainingOreUnits;
        Revenue = revenue;
        Rent = rent;
        Tax = tax;
        Profit = profit;
    }

    public decimal ElapsedTime { get; }

    public int TripCount { get; }

    public decimal MinedVolume { get; }

    public IReadOnlyDictionary<Mineral, decimal> Minerals => _minerals;

    public decimal RemainingOreUnits { get; }

    public decimal Revenue { get; }

    public decimal Rent { get; }

    public decimal Tax { get; }

    public decimal Profit { get; }
}
