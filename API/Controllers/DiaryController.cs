using API.Helpers;
using Application.Diaries;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DiaryController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetDiaries([FromQuery] DiaryParams diaryParams, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllDiaries(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiary(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [Authorize(Policy = Konstants.IsDiaryOwner)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDiary(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiray([FromBody] Diary diary, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiary(Guid id)
    {
        throw new NotImplementedException();
    }
}