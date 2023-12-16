using Task = Tasks.Application.Models.Task;

namespace Tasks.Application.Services;
public interface ITaskService
{
    Task<bool> CreateAsync(Task task, CancellationToken token = default);

    Task<Task?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<Task?> GetBySlugAsync(string slug, CancellationToken token = default);

    Task<IEnumerable<Task>> GetAllAsync(CancellationToken token = default);

    Task<Task?> UpdateAsync(Task task, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}