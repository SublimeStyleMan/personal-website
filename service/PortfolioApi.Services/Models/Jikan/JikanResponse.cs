using System.Text.Json.Serialization;

public class JikanResponse<T>
{
    /// <summary>
    /// The data returned by the Jikan API.
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}