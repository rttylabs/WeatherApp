using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class HourData
    {
        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("temp_c")]
        public double TempC { get; set; }

        [JsonPropertyName("condition")]
        public Condition? Condition { get; set; }

        [JsonPropertyName("chance_of_rain")]
        public int ChanceOfRain { get; set; }
    }
}
