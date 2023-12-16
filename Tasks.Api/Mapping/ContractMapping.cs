using Tasks.Contracts.Requests;
using Tasks.Contracts.Responses;
using Task = Tasks.Application.Models.Task;

namespace Tasks.Api.Mapping;

public static class ContractMapping
{
    public static Task MapToTask(this CreateTaskRequest request)
    {
        return new Task
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate,
            Tags = request.Tags.ToList()
        };
    }

    public static Task MapToTask(this UpdateTaskRequest request, Guid id)
    {
        return new Task
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate,
            Tags = request.Tags.ToList()
        };
    }

    public static TaskResponse MapToResponse(this Task task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            //Slug = task.Slug,
            Status = task.Status,
            DueDate = task.DueDate,
            Tags = task.Tags
        };
    }

    public static TasksResponse MapToResponse(this IEnumerable<Task> tasks)
    {
        return new TasksResponse
        {
            Items = tasks.Select(MapToResponse)
        };
    }
}

