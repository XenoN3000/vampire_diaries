using API.Helpers;
using Application.Diaries;
using Application.Tasks;
using Application.Tasks.DTOs;
using Application.Tasks.Handlers;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TaskController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] TaskParams taskParams, CancellationToken cancellationToken)
    {
        return HandlePagedResult(await Mediator.Send(new List.Query { Params = taskParams }, cancellationToken));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllTasks(CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new ListAll.Query(), cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Details.Query { Id = id }, cancellationToken));
    }

    [Authorize(Policy = Konstants.IsDiaryOwner)]
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