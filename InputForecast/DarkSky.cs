using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ForecastIO.Extensions;
using ForecastIO;


namespace WeatherForecast
{
    class DarkSky : InputForecast.AbstractInputWeather
    {

        RootObject weather;

        static DateTime ConvertFromUnixToDateTime(double unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return dateTime.AddSeconds(unixTime);
        }

        #region JSON Classes
        public class Currently
        {
            public int time { get; set; }
            public string summary { get; set; }
            public string icon { get; set; }
            public double precipIntensity { get; set; }
            public double precipProbability { get; set; }
            public string precipType { get; set; }
            public double precipAccumulation { get; set; }
            public double temperature { get; set; }
            public double apparentTemperature { get; set; }
            public double dewPoint { get; set; }
            public double humidity { get; set; }
            public double pressure { get; set; }
            public double windSpeed { get; set; }
            public double windGust { get; set; }
            public int windBearing { get; set; }
            public double cloudCover { get; set; }
            public int uvIndex { get; set; }
            public double visibility { get; set; }
            public double ozone { get; set; }
        }

        public class Datum
        {
            public int time { get; set; }
            public string summary { get; set; }
            public string icon { get; set; }
            public double precipIntensity { get; set; }
            public double precipProbability { get; set; }
            public string precipType { get; set; }
            public double precipAccumulation { get; set; }
            public double temperature { get; set; }
            public double apparentTemperature { get; set; }
            public double dewPoint { get; set; }
            public double humidity { get; set; }
            public double pressure { get; set; }
            public double windSpeed { get; set; }
            public double windGust { get; set; }
            public int windBearing { get; set; }
            public double cloudCover { get; set; }
            public int uvIndex { get; set; }
            public double visibility { get; set; }
            public double ozone { get; set; }
        }

        public class Hourly
        {
            public string summary { get; set; }
            public string icon { get; set; }
            public IList<Datum> data { get; set; }
        }

        public class Datum2
        {
            public int time { get; set; }
            public string summary { get; set; }
            public string icon { get; set; }
            public int sunriseTime { get; set; }
            public int sunsetTime { get; set; }
            public double moonPhase { get; set; }
            public double precipIntensity { get; set; }
            public double precipIntensityMax { get; set; }
            public int precipIntensityMaxTime { get; set; }
            public double precipProbability { get; set; }
            public string precipType { get; set; }
            public double precipAccumulation { get; set; }
            public double temperatureHigh { get; set; }
            public int temperatureHighTime { get; set; }
            public double temperatureLow { get; set; }
            public int temperatureLowTime { get; set; }
            public double apparentTemperatureHigh { get; set; }
            public int apparentTemperatureHighTime { get; set; }
            public double apparentTemperatureLow { get; set; }
            public int apparentTemperatureLowTime { get; set; }
            public double dewPoint { get; set; }
            public double humidity { get; set; }
            public double pressure { get; set; }
            public double windSpeed { get; set; }
            public double windGust { get; set; }
            public int windGustTime { get; set; }
            public int windBearing { get; set; }
            public double cloudCover { get; set; }
            public int uvIndex { get; set; }
            public int uvIndexTime { get; set; }
            public double visibility { get; set; }
            public double ozone { get; set; }
            public double temperatureMin { get; set; }
            public int temperatureMinTime { get; set; }
            public double temperatureMax { get; set; }
            public int temperatureMaxTime { get; set; }
            public double apparentTemperatureMin { get; set; }
            public int apparentTemperatureMinTime { get; set; }
            public double apparentTemperatureMax { get; set; }
            public int apparentTemperatureMaxTime { get; set; }
        }

        public class Daily
        {
            public string summary { get; set; }
            public string icon { get; set; }
            public IList<Datum2> data { get; set; }
        }

        public class Flags
        {
            public IList<string> sources { get; set; }
            public double neareststation { get; set; }
            public string units { get; set; }
        }

        public class RootObject
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
            public string timezone { get; set; }
            public Currently currently { get; set; }
            public Hourly hourly { get; set; }
            public Daily daily { get; set; }
            public Flags flags { get; set; }
            public int offset { get; set; }
        }
        #endregion
        public DarkSky(string token, string location) : base(token, location)
        {

        }

        private RootObject GetWeather() //https://darksky.net/dev/account
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebRequest requestBit = WebRequest.Create(@"https://api.darksky.net/forecast/" + token + "/" + location + "?units=si&lang=ru&extend=hourly");
            using (WebResponse response = requestBit.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string input = reader.ReadToEnd();

                        RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(input);

                        return rootObject;
                    }
                }
            }
        }

        public IList<WeatherClass>[] GetFullWeather()
        {
            if (weather == null || ConvertFromUnixToDateTime(weather.hourly.data[0].time).Day < DateTime.Now.Day)
            { weather = GetWeather(); }

            hourly = GetHourly(weather);
            threeHourly = GetThreeHourly(weather);
            daily = GetDaily(weather);
            return new IList<WeatherClass>[3] { hourly, threeHourly, daily };
        }

        private IList<WeatherClass> GetHourly(RootObject weather)
        {
            hourly = new List<WeatherClass>();

            foreach (var item in weather.hourly.data)
            {

                hourly.Add(new WeatherClass
                {
                    Date = ConvertFromUnixToDateTime(item.time),
                    TempMax = item.temperature,
                    TempMin = item.temperature,
                    WindDirection = item.windBearing,
                    WindSpeed = item.windSpeed,
                    WeatherCode = ChangeCode(item.icon),
                    PrecipProbability = item.precipProbability * 100,
                    Clouds = item.icon,
                    CloudsValue = item.cloudCover * 100,
                    Visibility = item.visibility
                });
            }
            return hourly;

        }

        private IList<WeatherClass> GetThreeHourly(RootObject weather)
        {
            threeHourly = new List<WeatherClass>();

            foreach (var item in weather.hourly.data)
            {
                if (ConvertFromUnixToDateTime(item.time).Hour % 3 == 0)
                {
                    threeHourly.Add(new WeatherClass
                    {
                        Date = ConvertFromUnixToDateTime(item.time),
                        TempMax = item.temperature,
                        TempMin = item.temperature,
                        WindDirection = item.windBearing,
                        WindSpeed = item.windSpeed,
                        WeatherCode = ChangeCode(item.icon),
                        PrecipProbability = item.precipProbability * 100,
                        Clouds = item.icon,
                        CloudsValue = item.cloudCover * 100,
                        Visibility = item.visibility
                    });
                }

            }

            return threeHourly;
        }

        private IList<WeatherClass> GetDaily(RootObject weather)
        {
            daily = new List<WeatherClass>();

            foreach (var item in weather.daily.data)
            {
                daily.Add(new WeatherClass
                {
                    Date = ConvertFromUnixToDateTime(item.time),
                    TempMax = item.temperatureMax,
                    TempMin = item.temperatureMin,
                    WindDirection = item.windBearing,
                    WindSpeed = item.windSpeed,
                    WeatherCode = ChangeCode(item.icon),
                    PrecipProbability = item.precipProbability * 100,
                    Clouds = item.icon,
                    CloudsValue = item.cloudCover * 100,
                    Visibility = item.visibility
                });

            }

            return daily;
        }
        int ChangeCode(string inputCode)
        {
            switch (inputCode)
            {
                case "rain":
                    return 502;
                case "snow":
                    return 601;
                case "sleet":
                    return 611;
                case "wind ":
                    return 700;
                case "fog":
                    return 741;
                case "clear-day":
                    return 800;
                case "clear-night":
                    return 800;
                case "partly-cloudy-day":
                    return 802;
                case "partly-cloudy-night":
                    return 802;
                case "cloudy":
                    return 804;

                default:
                    return 1;
            }

        } // This method changes the weather code to a common code.

    }
}
