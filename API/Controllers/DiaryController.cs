using API.Helpers;
using Application.Diaries;
using Application.Diaries.DTOs;
using Application.Diaries.Handlers;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DiaryController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetDiaries([FromQuery] DiaryParams diaryParams, CancellationToken cancellationToken)
    {
        return HandlePagedResult(await Mediator.Send(new List.Query { Params = diaryParams }, cancellationToken));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllDiaries(CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new ListAll.Query(), cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiary(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Details.Query { Id = id }, cancellationToken));
    }

    [Authorize(Policy = Konstants.IsDiaryOwner)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDiary(Guid id, [FromBody] DiaryDto diaryDto, CancellationToken cancellationToken)
    {
        diaryDto.Id = id;
        return HandleResult(await Mediator.Send(new Edit.Command { DiaryDto = diaryDto }, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiray([FromBody] CreateDiaryDto diary, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Create.Command { DiaryDto = diary }, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiary(Guid id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }, cancellationToken));
    }
}