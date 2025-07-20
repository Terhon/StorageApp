namespace Storage.Application.Commands;

public class UpdateItemTypeCommand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
}