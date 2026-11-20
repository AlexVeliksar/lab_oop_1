using Itmo.ObjectOrientedProgramming.Lab1.Models;
using System.Collections.ObjectModel;

namespace Itmo.ObjectOrientedProgramming.Lab1.Contracts;

public sealed class PriceList
{
    private readonly ReadOnlyDictionary<Mineral, decimal> _prices;

    public PriceList(IEnumerable<KeyValuePair<Mineral, decimal>> prices)
    {
        ArgumentNullException.ThrowIfNull(prices);
        var priceDictionary = new Dictionary<Mineral, decimal>();

        foreach (KeyValuePair<Mineral, decimal> price in prices)
        {
            if (price.Value < 0)
            {
                throw new ArgumentException("Цена минерала не может быть отрицательной.", nameof(prices));
            }

            if (!priceDictionary.TryAdd(price.Key, price.Value))
            {
                throw new ArgumentException("Цена минерала указана несколько раз.", nameof(prices));
            }
        }

        _prices = new ReadOnlyDictionary<Mineral, decimal>(priceDictionary);
    }

    public IReadOnlyDictionary<Mineral, decimal> Prices => _prices;

    public bool TryGetPrice(Mineral mineral, out decimal price)
    {
        return _prices.TryGetValue(mineral, out price);
    }
}
