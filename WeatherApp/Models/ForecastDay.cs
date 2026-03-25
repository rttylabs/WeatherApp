using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class ForecastDay
    {
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("day")]
        public DayData? Day { get; set; }

        [JsonPropertyName("hour")]
        public List<HourData>? Hour { get; set; }

        [JsonPropertyName("astro")]
        public Astro? Astro { get; set; }
    }
}
