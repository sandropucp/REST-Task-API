namespace Tasks.Contracts.Responses;

public class TaskResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Slug { get; init; }
    public required string Status { get; init; }
    public required DateTime DueDate { get; init; }
    public required IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
}