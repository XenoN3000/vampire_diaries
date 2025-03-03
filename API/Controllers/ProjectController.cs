using API.Helpers;
using Application.Projects;
using Application.Projects.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AppProject = Application.Projects.Handlers;

namespace API.Controllers;

public class ProjectController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetProjects([FromQuery] ProjectParams projectParams, CancellationToken cancellationToken)
    {
        return HandlePagedResult(await Mediator.Send(new AppProject.List.Query { Params = projectParams }, cancellationToken));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetProjectsAll(CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new AppProject.ListAll.Query(), cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new AppProject.Details.Query { Id = id }, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new AppProject.Create.Command { CreateProjectDto = createProjectDto }, cancellationToken));
    }

    [Authorize(Policy = Konstants.IsDiaryOwner)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectDto projectDto, CancellationToken cancellationToken)
    {
        projectDto.Id = id;
        return HandleResult(await Mediator.Send(new AppProject.Edit.Command { ProjectDto = projectDto }, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new AppProject.Delete.Command { Id = id }, cancellationToken));
    }
}