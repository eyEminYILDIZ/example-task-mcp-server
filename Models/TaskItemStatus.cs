using System.Text.Json.Serialization;

namespace example_mcp_server.Models;

[JsonConverter(typeof(JsonStringEnumConverter<TaskItemStatus>))]
public enum TaskItemStatus
{
    [JsonStringEnumMemberName("todo")]
    Todo,

    [JsonStringEnumMemberName("inprogress")]
    InProgress,

    [JsonStringEnumMemberName("done")]
    Done
}
