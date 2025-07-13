namespace Storage.Domain.Entities;

public class ItemType(int id, string name, string unit)
{
    public string Name { get; private set; } = name;
    public string Unit { get; private set; } = unit;
}