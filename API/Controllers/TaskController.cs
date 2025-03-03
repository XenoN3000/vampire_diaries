using API.Helpers;
using Application.Tasks;
using Application.Tasks.DTOs;
using Application.Tasks.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TaskController : BaseApiController
{
    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetTasks(Guid projectId, [FromQuery] TaskParams taskParams, CancellationToken cancellationToken)
    {
        return HandlePagedResult(await Mediator.Send(new List.Query { Params = taskParams, ProjectId = projectId}, cancellationToken));
    }

    [HttpGet("project/all/{projectId}")]
    public async Task<IActionResult> GetAllTasks(Guid projectId, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new ListAll.Query{ProjectId = projectId}, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Details.Query { Id = id }, cancellationToken));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskDto taskDto, CancellationToken cancellationToken)
    {
        taskDto.Id = id;
        return HandleResult(await Mediator.Send(new Edit.Command { TaskDto = taskDto }, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto task, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Create.Command { TaskDto = task }, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }, cancellationToken));
    }
}