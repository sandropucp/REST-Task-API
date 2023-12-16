using Dapper;
using Tasks.Application.Database;
using Task = Tasks.Application.Models.Task;

namespace Tasks.Application.Repositories;
public class TaskRepository : ITaskRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TaskRepository(IDbConnectionFactory dbConnectionFactory)
    {  
        _dbConnectionFactory = dbConnectionFactory; 
    }

    public async Task<bool> CreateAsync(Task task, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into tasks (id, slug, title, description, dueDate) 
            values (@Id, @Slug, @Title, @Description, @DueDate)
            """, task, cancellationToken: token));

        if (result > 0)
        {
            foreach (var tag in task.Tags)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    insert into tags (taskId, name) 
                    values (@TaskId, @Name)
                    """, new { TaskId = task.Id, Name = tag }, cancellationToken: token));
            }
        }

        transaction.Commit();

        return result > 0;
    }

    public async Task<Task?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var task = await connection.QuerySingleOrDefaultAsync<Task>(
            new CommandDefinition("""
            select * from tasks where id = @id
            """, new { id }, cancellationToken: token));

        if (task is null)
        {
            return null;
        }

        var tags = await connection.QueryAsync<string>(
            new CommandDefinition("""
            select name from tags where taskid = @id 
            """, new { id }, cancellationToken: token));

        foreach (var tag in tags)
        {
            task.Tags.Add(tag);
        }

        return task;
    }

    public async Task<Task?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var task = await connection.QuerySingleOrDefaultAsync<Task>(
            new CommandDefinition("""
            select * from tasks where slug = @slug
            """, new { slug }, cancellationToken: token));

        if (task is null)
        {
            return null;
        }

        var tags = await connection.QueryAsync<string>(
            new CommandDefinition("""
            select name from tags where tagid = @id 
            """, new { id = task.Id }, cancellationToken: token));

        foreach (var tag in tags)
        {
            task.Tags.Add(tag);
        }

        return task;
    }

    public async Task<IEnumerable<Task>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync(new CommandDefinition("""
            select m.*, string_agg(g.name, ',') as genres 
            from tasks m left join tags g on m.id = g.taskid
            group by id 
            """, cancellationToken: token));

        return result.Select(x => new Task
        {
            Id = x.id,
            Title = x.title,
            Description = x.description,
            Status = x.status,
            DueDate = x.date,
            Tags = Enumerable.ToList(x.tags.Split(','))
        });
    }

    public async Task<bool> UpdateAsync(Task task, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            delete from tags where tagid = @id
            """, new { id = task.Id }, cancellationToken: token));

        foreach (var tag in task.Tags)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                    insert into tags (tagId, name) 
                    values (@TaskId, @Name)
                    """, new { TaskId = task.Id, Name = tag }, cancellationToken: token));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            update tasks set slug = @Slug, title = @Title, description = @Description, date = @Date 
            where id = @Id
            """, task, cancellationToken: token));

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            delete from tags where taskid = @id
            """, new { id }, cancellationToken: token));

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            delete from tasks where id = @id
            """, new { id }, cancellationToken: token));

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            select count(1) from tasks where id = @id
            """, new { id }, cancellationToken: token));
    }
}
