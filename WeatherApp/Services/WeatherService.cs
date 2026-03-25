using System.Text.Json;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;
        private const string ApiKey = "fa8b3df74d4042b9aa7135114252304";
        private const string Latitude = "55.7558";
        private const string Longitude = "37.6173";

        public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<WeatherViewModel> GetWeatherAsync()
        {
            var viewModel = new WeatherViewModel { IsLoading = true, HasError = false };

            try
            {
                var currentUrl = $"http://api.weatherapi.com/v1/current.json?key={ApiKey}&q={Latitude},{Longitude}";
                var forecastUrl = $"http://api.weatherapi.com/v1/forecast.json?key={ApiKey}&q={Latitude},{Longitude}&days=3";

                var currentTask = await _httpClient.GetStringAsync(currentUrl);
                var forecastTask = await _httpClient.GetStringAsync(forecastUrl);

                var currentData = JsonSerializer.Deserialize<WeatherResponse>(currentTask);
                var forecastData = JsonSerializer.Deserialize<WeatherResponse>(forecastTask);

                if (currentData != null && forecastData != null)
                {
                    currentData.Forecast = forecastData.Forecast;
                    viewModel.Data = currentData;

                    // Обработка почасового прогноза
                    viewModel.HourlyForecast = ProcessHourlyForecast(forecastData.Forecast);
                    viewModel.ThreeDayForecast = forecastData.Forecast?.ForecastDays?.Take(3).ToList() ?? new List<ForecastDay>();
                }
                else
                {
                    viewModel.HasError = true;
                    viewModel.ErrorMessage = "Не удалось получить данные о погоде";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Ошибка сети при запросе погоды");
                viewModel.HasError = true;
                viewModel.ErrorMessage = "Ошибка сети. Проверьте подключение к интернету";
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Ошибка обработки данных");
                viewModel.HasError = true;
                viewModel.ErrorMessage = "Ошибка обработки данных погоды";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка");
                viewModel.HasError = true;
                viewModel.ErrorMessage = "Произошла неизвестная ошибка";
            }
            finally
            {
                viewModel.IsLoading = false;
            }

            return viewModel;
        }

        private List<HourData> ProcessHourlyForecast(Forecast? forecast)
        {
            var hourlyList = new List<HourData>();

            if (forecast?.ForecastDays == null || forecast.ForecastDays.Count == 0)
                return hourlyList;

            var now = DateTime.Now;
            var currentHour = now.Hour;

            // Сегодняшние часы, начиная с текущего
            var todayHours = forecast.ForecastDays[0].Hour
                .Where(h => DateTime.Parse(h.Time).Hour >= currentHour)
                .ToList();

            hourlyList.AddRange(todayHours);

            // Все часы следующего дня
            if (forecast.ForecastDays.Count > 1)
            {
                var nextDayHours = forecast.ForecastDays[1].Hour.ToList();
                hourlyList.AddRange(nextDayHours);
            }

            return hourlyList.Take(24).ToList(); // Показываем максимум 24 часа
        }
    }
}