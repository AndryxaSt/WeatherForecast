using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WeatherForecast
{

    class OpenWeather
    {
        readonly string token;
        public OpenWeather(string token)
        {
            this.token = token;
        }
        private double ConvertDirectionToBearing(string directionInput)
        {


            string direction = directionInput.ToLower();

            if (direction == "с" || direction == "n")
            {
                return 0;
            }

            if (direction == "ссв" || direction == "nne")
            {
                return 22.5;
            }

            if (direction == "св" || direction == "ne")
            {
                return 45;
            }

            if (direction == "всв" || direction == "ene")
            {
                return 67.5;
            }

            if (direction == "в" || direction == "e")
            {
                return 90;
            }

            if (direction == "вюв" || direction == "ese")
            {
                return 112.5;
            }

            if (direction == "юв" || direction == "se")
            {
                return 135;
            }

            if (direction == "ююв" || direction == "sse")
            {
                return 157.5;
            }
            if (direction == "ю" || direction == "s")
            {
                return 180;
            }

            if (direction == "ююз" || direction == "ssw")
            {
                return 202.5;
            }

            if (direction == "юз" || direction == "sw")
            {
                return 225;
            }

            if (direction == "зюз" || direction == "wsw")
            {
                return 247.5;
            }
            if (direction == "з" || direction == "w")
            {
                return 270;
            }

            if (direction == "зсз" || direction == "wnw")
            {
                return 292.5;
            }

            if (direction == "сз" || direction == "nw")
            {
                return 315;
            }

            if (direction == "ссз" || direction == "nnw")
            {
                return 337.5;
            }

            return 0;
        }
        public async Task<List<WeatherClass>> GetWeather()
        {
            OpenWeatherMapClient client = new OpenWeatherMapClient(token);
            var currentWeather = await client.Forecast.GetByCityId(686875, false, MetricSystem.Metric, OpenWeatherMapLanguage.RU);

            return ConvertToWeatherClass(currentWeather.Forecast);

        }

        private List<WeatherClass> ConvertToWeatherClass(ForecastTime[] forecast)
        {
            List<WeatherClass> ThreeHourly = new List<WeatherClass>();

            foreach (var item in forecast)
            {
                ThreeHourly.Add(new WeatherClass
                {
                    Date = item.From,
                    TempMax = item.Temperature.Max,
                    TempMin = item.Temperature.Min,
                    WindDirection = ConvertDirectionToBearing(item.WindDirection.Code),
                    WindSpeed = item.WindSpeed.Mps,
                    WeatherCode = item.Symbol.Number,
                    Clouds = item.Clouds.Value,
                    CloudsValue = item.Clouds.All
                });
            }

            return ThreeHourly;
        }


    }
}
