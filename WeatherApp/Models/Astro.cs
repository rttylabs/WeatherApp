using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class Astro
    {
        [JsonPropertyName("sunrise")]
        public string? Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public string? Sunset { get; set; }
    }
}
