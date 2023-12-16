namespace Tasks.Contracts.Responses;
public class TasksResponse
{
    public required IEnumerable<TaskResponse> Items { get; init; } = Enumerable.Empty<TaskResponse>();
}