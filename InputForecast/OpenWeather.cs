using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WeatherForecast
{

    class OpenWeather : InputForecast.AbstractInputWeather
    {

        public OpenWeather(string token, string location) : base(token, location)
        {
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
        public async Task<IList<WeatherClass>> GetWeather()
        {
            OpenWeatherMapClient client = new OpenWeatherMapClient(token);
            var currentWeather = await client.Forecast.GetByCoordinates(new Coordinates() { Longitude = Convert.ToDouble(location.Split(',')[0].Replace('.', ',')), Latitude = Convert.ToDouble(location.Split(',')[1].Replace('.', ',')) },
                                                                        false,
                                                                        MetricSystem.Metric,
                                                                        OpenWeatherMapLanguage.RU);

            return ConvertToWeatherClass(currentWeather.Forecast);

        }

        private IList<WeatherClass> ConvertToWeatherClass(ForecastTime[] forecast)
        {
            threeHourly = new List<WeatherClass>();

            foreach (var item in forecast)
            {
                threeHourly.Add(new WeatherClass
                {
                    Date = item.From,
                    TempMax = item.Temperature.Max,
                    TempMin = item.Temperature.Min,
                    WindDirection = ConvertDirectionToBearing(item.WindDirection.Code),
                    WindSpeed = item.WindSpeed.Mps,
                    WeatherCode = ChangeCode(item.Symbol.Number),
                    Clouds = item.Clouds.Value,
                    CloudsValue = item.Clouds.All
                });
            }

            return threeHourly;
        }

        int ChangeCode(int inputCode)
        {
            switch (inputCode)
            {
                case 200:
                    return 200;
                case 201:
                    return 201;
                case 202:
                    return 202;
                case 210:
                    return 200;
                case 211:
                    return 201;
                case 212:
                    return 202;
                case 221:
                    return 202;
                case 230:
                    return 230;
                case 231:
                    return 231;
                case 232:
                    return 232;
                case 300:
                    return 300;
                case 301:
                    return 301;
                case 302:
                    return 302;
                case 310:
                    return 300;
                case 311:
                    return 301;
                case 312:
                    return 302;
                case 313:
                    return 300;
                case 314:
                    return 301;
                case 321:
                    return 302;
                case 500:
                    return 500;
                case 501:
                    return 500;
                case 502:
                    return 501;
                case 503:
                    return 501;
                case 504:
                    return 502;
                case 511:
                    return 511;
                case 520:
                    return 520;
                case 521:
                    return 521;
                case 522:
                    return 522;
                case 531:
                    return 522;
                case 600:
                    return 600;
                case 601:
                    return 601;
                case 602:
                    return 602;
                case 611:
                    return 611;
                case 612:
                    return 611;
                case 613:
                    return 612;
                case 615:
                    return 511;
                case 616:
                    return 610;
                case 620:
                    return 520;
                case 621:
                    return 621;
                case 622:
                    return 622;
                case 701:
                    return 700;
                case 711:
                    return 711;
                case 721:
                    return 721;
                case 731:
                    return 731;
                case 741:
                    return 741;
                case 751:
                    return 751;
                case 761:
                    return 731;
                case 762:
                    return 731;
                case 771:
                    return 731;
                case 781:
                    return 731;
                case 800:
                    return 800;
                case 801:
                    return 801;
                case 802:
                    return 802;
                case 803:
                    return 803;
                case 804:
                    return 804;

                default:
                    return 1;


            }
        }// This method changes the weather code to a common code.

    }
}
