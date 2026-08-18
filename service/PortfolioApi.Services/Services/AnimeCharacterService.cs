using System.Net.Http.Json;
using PortfolioApi.Services.Interfaces;
using PortfolioApi.Services.Models.Jikan;

namespace PortfolioApi.Services;
public class AnimeCharacterService : IAnimeCharacterService
{
    private readonly HttpClient _httpClient;

    public AnimeCharacterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AnimeCharacterDto>> SearchCharactersAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        var url = $"characters?q={Uri.EscapeDataString(query)}&limit=20";

        var response = await _httpClient.GetFromJsonAsync<
            JikanResponse<List<JikanCharacter>>
        >(url, cancellationToken);

        if (response?.Data == null)
            return [];

        return response.Data
            .Select(MapCharacter)
            .ToList();
    }

    public async Task<AnimeCharacterDto?> GetCharacterAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<
            JikanResponse<JikanCharacter>
        >($"characters/{id}", cancellationToken);

        if (response?.Data == null)
            return null;

        return MapCharacter(response.Data);
    }

    private static AnimeCharacterDto MapCharacter(JikanCharacter character)
    {
        return new AnimeCharacterDto
        {
            Id = character.MalId,
            Name = character.Name,
            NameKanji = character.NameKanji,
            About = character.About,
            ImageUrl = character.Images?.Jpg?.ImageUrl,
            LargeImageUrl = character.Images?.Jpg?.LargeImageUrl
        };
    }
}