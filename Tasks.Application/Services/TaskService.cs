using FluentValidation;
using Tasks.Application.Repositories;
using Task = Tasks.Application.Models.Task;

namespace Tasks.Application.Services;
public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IValidator<Task> _taskValidator;

    public TaskService(ITaskRepository taskRepository, IValidator<Task> taskValidator)
    {
        _taskRepository = taskRepository;
        _taskValidator = taskValidator;
    }

    public async Task<bool> CreateAsync(Task task, CancellationToken token = default)
    {
        await _taskValidator.ValidateAndThrowAsync(task, cancellationToken: token);
        return await _taskRepository.CreateAsync(task, token);
    }

    public Task<Task?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _taskRepository.GetByIdAsync(id, token);
    }

    public Task<Task?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        return _taskRepository.GetBySlugAsync(slug, token);
    }

    public Task<IEnumerable<Task>> GetAllAsync(CancellationToken token = default)
    {
        return _taskRepository.GetAllAsync(token);
    }

    public async Task<Task?> UpdateAsync(Task task, CancellationToken token = default)
    {
        await _taskValidator.ValidateAndThrowAsync(task, cancellationToken: token);
        var taskExists = await _taskRepository.ExistsByIdAsync(task.Id, token);
        if (!taskExists)
        {
            return null;
        }

        await _taskRepository.UpdateAsync(task, token);
        return task;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _taskRepository.DeleteByIdAsync(id, token);
    }
}