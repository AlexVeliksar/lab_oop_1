namespace Itmo.ObjectOrientedProgramming.Lab1.Ships;

public sealed class Ship
{
    public Ship(
        string name,
        decimal cargoCapacity,
        decimal speed,
        decimal hourlyRent,
        IMiningBehavior miningBehavior)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название корабля не может быть пустым.", nameof(name));
        }

        if (cargoCapacity <= 0)
        {
            throw new ArgumentException("Объём трюма должен быть положительным.", nameof(cargoCapacity));
        }

        if (speed <= 0)
        {
            throw new ArgumentException("Скорость должна быть положительной.", nameof(speed));
        }

        if (hourlyRent < 0)
        {
            throw new ArgumentException("Стоимость аренды не может быть отрицательной.", nameof(hourlyRent));
        }

        Name = name;
        CargoCapacity = cargoCapacity;
        Speed = speed;
        HourlyRent = hourlyRent;
        MiningBehavior = miningBehavior ?? throw new ArgumentNullException(nameof(miningBehavior));
    }

    public string Name { get; }

    public decimal CargoCapacity { get; }

    public decimal Speed { get; }

    public decimal HourlyRent { get; }

    public IMiningBehavior MiningBehavior { get; }
}
