using Carsales.Bff.Api.Models;
using Carsales.Bff.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Carsales.Bff.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class EpisodesController : ControllerBase
{
    private readonly IRickAndMortyService _service;

    public EpisodesController(IRickAndMortyService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<EpisodeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<EpisodeDto>>> Get([FromQuery] int page = 1, [FromQuery] string? name = null, CancellationToken ct = default)
    {
        if (page < 1) return BadRequest("`page` debe ser >= 1.");
        var result = await _service.GetEpisodesAsync(page, name, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EpisodeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EpisodeDto>> GetById(int id, CancellationToken ct)
    {
        var ep = await _service.GetEpisodeByIdAsync(id, ct);
        return ep is null ? NotFound() : Ok(ep);
    }
}
