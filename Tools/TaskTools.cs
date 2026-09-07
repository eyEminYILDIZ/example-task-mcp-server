using System.ComponentModel;
using example_mcp_server.Models;
using example_mcp_server.Services;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace example_mcp_server.Tools;

[McpServerToolType]
public sealed class TaskTools
{
    private readonly ITaskService _taskService;

    public TaskTools(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [McpServerTool(Name = "get_all_tasks")]
    [Description("Gets every personal task, regardless of status.")]
    public IReadOnlyList<TaskItem> GetAllTasks() => _taskService.GetAll();

    [McpServerTool(Name = "get_tasks_by_status")]
    [Description("Gets personal tasks filtered by status.")]
    public IReadOnlyList<TaskItem> GetTasksByStatus(
        [Description("Status to filter by: todo, inprogress, or done.")] string status)
    {
        var parsed = TaskStatusParser.Parse(status);
        return _taskService.GetByStatus(parsed);
    }

    [McpServerTool(Name = "create_task")]
    [Description("Creates a new personal task. New tasks always start with status todo.")]
    public TaskItem CreateTask(
        [Description("Short title of the task.")] string title,
        [Description("Optional longer description of the task.")] string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new McpException("Title is required and cannot be empty.");
        }

        return _taskService.Create(title.Trim(), string.IsNullOrWhiteSpace(description) ? null : description.Trim());
    }

    [McpServerTool(Name = "change_task_status")]
    [Description("Changes the status of an existing task (e.g. move it to inprogress or done).")]
    public TaskItem ChangeTaskStatus(
        [Description("Id of the task to update.")] int id,
        [Description("New status: todo, inprogress, or done.")] string status)
    {
        var parsed = TaskStatusParser.Parse(status);
        return _taskService.ChangeStatus(id, parsed);
    }

    [McpServerTool(Name = "delete_task")]
    [Description("Permanently deletes a task.")]
    public string DeleteTask([Description("Id of the task to delete.")] int id)
    {
        if (!_taskService.Delete(id))
        {
            throw new McpException($"No task found with id {id}.");
        }

        return $"Task {id} deleted.";
    }
}
