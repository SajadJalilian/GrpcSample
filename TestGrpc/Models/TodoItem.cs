namespace TestGrpc.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Descreption { get; set; }
    public string TodoStatus { get; set; } = "NEW";
}