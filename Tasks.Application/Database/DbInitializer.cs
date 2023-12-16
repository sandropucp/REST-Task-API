using Dapper;

namespace Tasks.Application.Database;
public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
            create table if not exists tasks (
            id UUID primary key,
            slug TEXT not null, 
            title TEXT not null,
            description TEXT not null,
            dueDate integer not null);
        """);

        await connection.ExecuteAsync("""
            create unique index concurrently if not exists tasks_slug_idx
            on tasks
            using btree(slug);
        """);

        await connection.ExecuteAsync("""
            create table if not exists tags (
            taskId UUID references tasks (Id),
            name TEXT not null);
        """);
    }
}