using example_mcp_server.Models;

namespace example_mcp_server.Services;

public interface ITaskService
{
    IReadOnlyList<TaskItem> GetAll();
    IReadOnlyList<TaskItem> GetByStatus(TaskItemStatus status);
    TaskItem Create(string title, string? description);
    TaskItem ChangeStatus(int id, TaskItemStatus status);
    bool Delete(int id);
}
