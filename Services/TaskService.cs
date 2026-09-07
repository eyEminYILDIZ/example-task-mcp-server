using System.Collections.Concurrent;
using example_mcp_server.Models;
using ModelContextProtocol;

namespace example_mcp_server.Services;

/// <summary>
/// In-memory store for the user's personal tasks, seeded with a small static
/// list on startup. Not persisted across restarts.
/// </summary>
public sealed class TaskService : ITaskService
{
    private readonly ConcurrentDictionary<int, TaskItem> _tasks = new();
    private int _nextId;

    public TaskService()
    {
        var now = DateTimeOffset.UtcNow;

        Seed(new TaskItem
        {
            Id = NextId(),
            Title = "Plan weekly groceries",
            Description = "Write down what's needed for the week and check the pantry first.",
            Status = TaskItemStatus.Todo,
            CreatedAt = now,
            UpdatedAt = now
        });

        Seed(new TaskItem
        {
            Id = NextId(),
            Title = "Finish MCP server example",
            Description = "Implement the task tools and verify them with an MCP client.",
            Status = TaskItemStatus.InProgress,
            CreatedAt = now,
            UpdatedAt = now
        });

        Seed(new TaskItem
        {
            Id = NextId(),
            Title = "Renew gym membership",
            Description = null,
            Status = TaskItemStatus.Done,
            CreatedAt = now,
            UpdatedAt = now
        });
    }

    private void Seed(TaskItem task) => _tasks[task.Id] = task;

    private int NextId() => Interlocked.Increment(ref _nextId);

    public IReadOnlyList<TaskItem> GetAll() =>
        _tasks.Values.OrderBy(t => t.Id).ToList();

    public IReadOnlyList<TaskItem> GetByStatus(TaskItemStatus status) =>
        _tasks.Values.Where(t => t.Status == status).OrderBy(t => t.Id).ToList();

    public TaskItem Create(string title, string? description)
    {
        var now = DateTimeOffset.UtcNow;
        var task = new TaskItem
        {
            Id = NextId(),
            Title = title,
            Description = description,
            Status = TaskItemStatus.Todo,
            CreatedAt = now,
            UpdatedAt = now
        };

        _tasks[task.Id] = task;
        return task;
    }

    public TaskItem ChangeStatus(int id, TaskItemStatus status)
    {
        while (true)
        {
            if (!_tasks.TryGetValue(id, out var existing))
            {
                throw new McpException($"No task found with id {id}.");
            }

            var updated = existing with { Status = status, UpdatedAt = DateTimeOffset.UtcNow };

            if (_tasks.TryUpdate(id, updated, existing))
            {
                return updated;
            }
        }
    }

    public bool Delete(int id) => _tasks.TryRemove(id, out _);
}
