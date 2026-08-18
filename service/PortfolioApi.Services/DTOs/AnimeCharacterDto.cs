public class AnimeCharacterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameKanji { get; set; }
    public string? ImageUrl { get; set; }
    public string? LargeImageUrl { get; set; }
    public string? About { get; set; }
}