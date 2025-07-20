namespace Storage.Application.Commands;

public class DeleteItemTypeCommand(int id)
{
    public int Id { get; } = id;
}