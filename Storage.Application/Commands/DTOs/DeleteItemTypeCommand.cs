namespace Storage.Application.Commands.DTOs;

public class DeleteItemTypeCommand(int id)
{
    public int Id { get; } = id;
}