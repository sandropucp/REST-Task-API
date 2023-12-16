using Task = Tasks.Application.Models.Task;

namespace Tasks.Application.Repositories;

public interface ITaskRepository
{
    Task<bool> CreateAsync(Task task, CancellationToken token = default);

    Task<Task?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<Task?> GetBySlugAsync(string slug, CancellationToken token = default);

    Task<IEnumerable<Task>> GetAllAsync(CancellationToken token = default);

    Task<bool> UpdateAsync(Task task, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
}
