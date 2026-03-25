namespace WeatherApp.Models
{
    public class WeatherViewModel
    {
        public WeatherResponse? Data { get; set; }
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsLoading { get; set; }
        public List<HourData>? HourlyForecast { get; set; }
        public List<ForecastDay>? ThreeDayForecast { get; set; }
    }
}
