using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfileController : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(string id, CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Application.Profiles.Handlers.Details.Query { DeviceId = id }, cancellationToken));
    }
}