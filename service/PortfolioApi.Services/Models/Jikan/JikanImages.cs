using System.Text.Json.Serialization;

public class JikanImages
{
    [JsonPropertyName("jpg")]
    public JikanImage? Jpg { get; set; }

    [JsonPropertyName("webp")]
    public JikanImage? Webp { get; set; }
}