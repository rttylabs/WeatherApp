using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class Forecast
    {
        [JsonPropertyName("forecastday")]
        public List<ForecastDay>? ForecastDays { get; set; }
    }
}
