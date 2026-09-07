# example-mcp-server

An MCP (Model Context Protocol) server that serves a personal task list. Built with
ASP.NET Core (.NET 10) and the [`ModelContextProtocol.AspNetCore`](https://www.nuget.org/packages/ModelContextProtocol.AspNetCore) SDK, using
HTTP transport.

Tasks are held in memory (seeded with a few sample tasks on startup) and support
`todo` / `inprogress` / `done` statuses. There is no assignee concept — these are
personal tasks.

## Tools

| Tool | Description |
|---|---|
| `get_all_tasks` | Gets every task, regardless of status. |
| `get_tasks_by_status` | Gets tasks filtered by status (`todo`, `inprogress`, `done`). |
| `create_task` | Creates a new task (starts as `todo`). |
| `change_task_status` | Changes a task's status. |
| `delete_task` | Permanently deletes a task. |

## Running the server

```bash
dotnet run
```

The MCP endpoint is served at `http://localhost:5144/mcp`.

## Using it with Claude

### Claude Code (CLI)

1. Start the server (from the project directory):
   ```bash
   dotnet run
   ```

2. In a separate terminal, register it:
   ```bash
   claude mcp add --transport http tasks http://localhost:5144/mcp
   ```

3. Verify it's connected:
   ```bash
   claude mcp list
   ```
   or, inside a Claude Code session, run `/mcp`.

Once added, tools show up as `mcp__tasks__get_all_tasks`, `mcp__tasks__create_task`, etc.
You can then just ask things like "show me my todo tasks" or "mark task 2 as done".

Notes:
- `claude mcp add` defaults to **local scope** (only this project directory). Add
  `--scope user` to make it available everywhere, or `--scope project` to share it via
  a checked-in `.mcp.json`.
- The server must actually be running (`dotnet run`) for the tools to work — it isn't
  launched automatically the way a stdio server would be.

### Claude Desktop

Desktop expects stdio servers by default, but recent versions support remote/HTTP
servers via **Settings → Connectors → Add custom connector**, where you'd paste
`http://localhost:5144/mcp`.

## Using it with GitHub Copilot CLI

1. Start the server (from the project directory):
   ```bash
   dotnet run
   ```

2. In a separate terminal, register it:
   ```bash
   copilot mcp add --transport http tasks http://localhost:5144/mcp
   ```

   Or interactively: run `copilot`, then inside the session type `/mcp add` and fill in
   the form (server type `HTTP`, URL `http://localhost:5144/mcp`, tools `*`), then
   press `Ctrl+S` to save.

3. This writes to `~/.copilot/mcp-config.json`:
   ```json
   {
     "mcpServers": {
       "tasks": {
         "type": "http",
         "url": "http://localhost:5144/mcp",
         "tools": ["*"]
       }
     }
   }
   ```

4. Verify it's connected by running `/mcp` inside a Copilot CLI session.

As with Claude Code, the server has to actually be running (`dotnet run`) for the
tools to work.

## Using it with VS Code

Requires the GitHub Copilot extension (Agent mode uses MCP tools in chat).

1. Start the server (from the project directory):
   ```bash
   dotnet run
   ```

2. Create `.vscode/mcp.json` in the project (or run **"MCP: Add Server"** from the
   Command Palette and choose **Workspace**):
   ```json
   {
     "servers": {
       "tasks": {
         "type": "http",
         "url": "http://localhost:5144/mcp"
       }
     }
   }
   ```

   To make it available across all workspaces instead, run **"MCP: Open User
   Configuration"** and add the same entry there.

3. VS Code shows a trust confirmation dialog the first time the server starts. Use
   **"MCP: List Servers"** from the Command Palette to check its status or restart it.

4. Open Copilot Chat, switch to **Agent mode**, and the `tasks` tools become available
   automatically — e.g. ask "show me my todo tasks" or "mark task 2 as done".

As with the other clients, the server has to actually be running (`dotnet run`) for
the tools to work.
