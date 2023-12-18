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
            drop table if exists tasks cascade;
            create table tasks (
            id UUID primary key,
            slug TEXT not null, 
            title TEXT not null,
            description TEXT not null,
            status TEXT not null,
            dueDate timestamp not null);
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