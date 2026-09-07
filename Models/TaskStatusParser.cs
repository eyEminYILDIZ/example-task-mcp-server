using ModelContextProtocol;

namespace example_mcp_server.Models;

public static class TaskStatusParser
{
    public static TaskItemStatus Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new McpException("Status is required. Valid values: todo, inprogress, done.");
        }

        var normalized = new string(value.Where(char.IsLetter).ToArray()).ToLowerInvariant();

        return normalized switch
        {
            "todo" => TaskItemStatus.Todo,
            "inprogress" => TaskItemStatus.InProgress,
            "done" => TaskItemStatus.Done,
            _ => throw new McpException($"Invalid status '{value}'. Valid values: todo, inprogress, done.")
        };
    }
}
