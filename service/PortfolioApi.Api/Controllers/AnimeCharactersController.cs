using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Services.Interfaces;

namespace PortfolioApi.Api.Controllers;

[ApiController]
[Route("api/anime/characters")]
public class AnimeCharactersController : ControllerBase
{
    private readonly IAnimeCharacterService _characterService;

    public AnimeCharactersController(
        IAnimeCharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<AnimeCharacterDto>>> Search(
        [FromQuery] string query,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required.");

        var characters = await _characterService
            .SearchCharactersAsync(query, cancellationToken);

        return Ok(characters);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnimeCharacterDto>> Get(
        int id,
        CancellationToken cancellationToken)
    {
        var character = await _characterService
            .GetCharacterAsync(id, cancellationToken);

        if (character == null)
            return NotFound();

        return Ok(character);
    }
}