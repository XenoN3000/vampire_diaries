using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfileController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        return HandleResult(await Mediator.Send(new Application.Profiles.Handlers.Details.Query(), cancellationToken));
    }
}