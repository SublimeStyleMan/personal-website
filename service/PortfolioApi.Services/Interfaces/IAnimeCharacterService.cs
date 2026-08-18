namespace PortfolioApi.Services.Interfaces;
public interface IAnimeCharacterService
{
    Task<List<AnimeCharacterDto>> SearchCharactersAsync(
        string query,
        CancellationToken cancellationToken = default);

    Task<AnimeCharacterDto?> GetCharacterAsync(
        int id,
        CancellationToken cancellationToken = default);
}