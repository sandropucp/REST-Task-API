namespace Tasks.Contracts.Requests;
public class CreateTaskRequest
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Status { get; init; }
    public required DateTime DueDate { get; init; }
    public required IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
}
