using System.Text.Json.Serialization;

namespace PortfolioApi.Services.Models.Jikan;

public class JikanCharacter
{
    [JsonPropertyName("mal_id")]
    public int MalId { get; set; }

    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("name_kanji")]
    public string? NameKanji { get; set; }

    public JikanImages? Images { get; set; }

    public string? About { get; set; }
}