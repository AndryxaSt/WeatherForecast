using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace WeatherForecast
{
    class AccuWeather : InputForecast.AbstractInputWeather

    {   //5day forecast http://dataservice.accuweather.com/forecasts/v1/daily/5day/325693?apikey="+token+"&language=ru-ru&metric=true
        //12hourly forecast http://dataservice.accuweather.com/forecasts/v1/hourly/12hour/325693?apikey="+token+"&language=ru-ru&metric=true


        public AccuWeather(string token, string location) : base (token, location)
        {

        }

        #region JsonClasses
        public class Temperature
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class RealFeelTemperature
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class WetBulbTemperature
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class DewPoint
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Speed
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Direction
        {
            public int Degrees { get; set; }
            public string Localized { get; set; }
            public string English { get; set; }
        }

        public class Wind
        {
            public Speed Speed { get; set; }
            public Direction Direction { get; set; }
        }

        public class WindGust
        {
            public Speed Speed { get; set; }
        }

        public class Visibility
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Ceiling
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class TotalLiquid
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Rain
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Snow
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Ice
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Hourly
        {
            public DateTime DateTime { get; set; }
            public int EpochDateTime { get; set; }
            public int WeatherIcon { get; set; }
            public string IconPhrase { get; set; }
            public bool HasPrecipitation { get; set; }
            public bool IsDaylight { get; set; }
            public Temperature Temperature { get; set; }
            public RealFeelTemperature RealFeelTemperature { get; set; }
            public WetBulbTemperature WetBulbTemperature { get; set; }
            public DewPoint DewPoint { get; set; }
            public Wind Wind { get; set; }
            public WindGust WindGust { get; set; }
            public int RelativeHumidity { get; set; }
            public int IndoorRelativeHumidity { get; set; }
            public Visibility Visibility { get; set; }
            public Ceiling Ceiling { get; set; }
            public int UVIndex { get; set; }
            public string UVIndexText { get; set; }
            public int PrecipitationProbability { get; set; }
            public int RainProbability { get; set; }
            public int SnowProbability { get; set; }
            public int IceProbability { get; set; }
            public TotalLiquid TotalLiquid { get; set; }
            public Rain Rain { get; set; }
            public Snow Snow { get; set; }
            public Ice Ice { get; set; }
            public int CloudCover { get; set; }
            public string MobileLink { get; set; }
            public string Link { get; set; }
        }


        public class Headline
        {
            public DateTime EffectiveDate { get; set; }
            public int EffectiveEpochDate { get; set; }
            public int Severity { get; set; }
            public string Text { get; set; }
            public string Category { get; set; }
            public DateTime EndDate { get; set; }
            public int EndEpochDate { get; set; }
            public string MobileLink { get; set; }
            public string Link { get; set; }
        }

        public class Sun
        {
            public DateTime Rise { get; set; }
            public int EpochRise { get; set; }
            public DateTime Set { get; set; }
            public int EpochSet { get; set; }
        }

        public class Moon
        {
            public DateTime Rise { get; set; }
            public int EpochRise { get; set; }
            public DateTime Set { get; set; }
            public int EpochSet { get; set; }
            public string Phase { get; set; }
            public int Age { get; set; }
        }

        public class Minimum
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Maximum
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }



        public class RealFeelTemperatureShade
        {
            public Minimum Minimum { get; set; }
            public Maximum Maximum { get; set; }
        }

        public class Heating
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Cooling
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class DegreeDaySummary
        {
            public Heating Heating { get; set; }
            public Cooling Cooling { get; set; }
        }

        public class AirAndPollen
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public string Category { get; set; }
            public int CategoryValue { get; set; }
            public string Type { get; set; }
        }

        public class Day
        {
            public int Icon { get; set; }
            public string IconPhrase { get; set; }
            public bool HasPrecipitation { get; set; }
            public string ShortPhrase { get; set; }
            public string LongPhrase { get; set; }
            public int PrecipitationProbability { get; set; }
            public int ThunderstormProbability { get; set; }
            public int RainProbability { get; set; }
            public int SnowProbability { get; set; }
            public int IceProbability { get; set; }
            public Wind Wind { get; set; }
            public WindGust WindGust { get; set; }
            public TotalLiquid TotalLiquid { get; set; }
            public Rain Rain { get; set; }
            public Snow Snow { get; set; }
            public Ice Ice { get; set; }
            public double HoursOfPrecipitation { get; set; }
            public double HoursOfRain { get; set; }
            public double HoursOfSnow { get; set; }
            public double HoursOfIce { get; set; }
            public int CloudCover { get; set; }
            public string PrecipitationType { get; set; }
            public string PrecipitationIntensity { get; set; }
        }

        public class Night
        {
            public int Icon { get; set; }
            public string IconPhrase { get; set; }
            public bool HasPrecipitation { get; set; }
            public string ShortPhrase { get; set; }
            public string LongPhrase { get; set; }
            public int PrecipitationProbability { get; set; }
            public int ThunderstormProbability { get; set; }
            public int RainProbability { get; set; }
            public int SnowProbability { get; set; }
            public int IceProbability { get; set; }
            public Wind Wind { get; set; }
            public WindGust WindGust { get; set; }
            public TotalLiquid TotalLiquid { get; set; }
            public Rain Rain { get; set; }
            public Snow Snow { get; set; }
            public Ice Ice { get; set; }
            public double HoursOfPrecipitation { get; set; }
            public double HoursOfRain { get; set; }
            public double HoursOfSnow { get; set; }
            public double HoursOfIce { get; set; }
            public int CloudCover { get; set; }
        }

        public class DailyForecast
        {
            public DateTime Date { get; set; }
            public int EpochDate { get; set; }
            public Sun Sun { get; set; }
            public Moon Moon { get; set; }
            public Temperature Temperature { get; set; }
            public RealFeelTemperature RealFeelTemperature { get; set; }
            public RealFeelTemperatureShade RealFeelTemperatureShade { get; set; }
            public double HoursOfSun { get; set; }
            public DegreeDaySummary DegreeDaySummary { get; set; }
            public IList<AirAndPollen> AirAndPollen { get; set; }
            public Day Day { get; set; }
            public Night Night { get; set; }
            public IList<string> Sources { get; set; }
            public string MobileLink { get; set; }
            public string Link { get; set; }
        }

        public class Daily
        {
            public Headline Headline { get; set; }
            public IList<DailyForecast> DailyForecasts { get; set; }
        }
        #endregion
        private IList<Hourly> GetWeatherHourly()
        {
            WebRequest requestBit = WebRequest.Create(@"http://dataservice.accuweather.com/forecasts/v1/hourly/12hour/" + location + "?apikey=" + token + "&language=ru-ru&details=true&metric=true");//details
            using (WebResponse response = requestBit.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string input = reader.ReadToEnd();

                        var hourly = JsonConvert.DeserializeObject<List<Hourly>>(input);

                        return hourly;
                    }
                }
            }
        }

        private IList<DailyForecast> GetWeatherDaily()
        {
            WebRequest requestBit = WebRequest.Create(@"http://dataservice.accuweather.com/forecasts/v1/daily/5day/" + location + "?apikey=" + token + "&language=ru-ru&details=true&metric=true");
            using (WebResponse response = requestBit.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string input = reader.ReadToEnd();

                        var temp = JsonConvert.DeserializeObject<Daily>(input);
                        IList<DailyForecast> daily = temp.DailyForecasts;
                        return daily;
                    }
                }
            }


        }

        public IList<WeatherClass>[] GetWeather()
        {
            IList<WeatherClass>[] fullWeather = new List<WeatherClass>[2];

            fullWeather[0] = ConvertHourlyToWeatherClass(GetWeatherHourly());
            fullWeather[1] = ConvertDailyToWeatherClass(GetWeatherDaily());

            return fullWeather;
        }

        private IList<WeatherClass> ConvertHourlyToWeatherClass(IList<Hourly> list)
        {
            hourly = new List<WeatherClass>();
            foreach (var hour in list)
            {
                hourly.Add(new WeatherClass
                {
                    Date = hour.DateTime,
                    TempMax = hour.Temperature.Value,
                    TempMin = hour.Temperature.Value,
                    WindDirection = hour.Wind.Direction.Degrees,
                    WindDirectionString = hour.Wind.Direction.Localized,
                    WindSpeed = hour.Wind.Speed.Value,
                    WeatherCode = ChangeCode(hour.WeatherIcon),
                    PrecipProbability = hour.PrecipitationProbability,
                    Clouds = hour.IconPhrase,
                    CloudsValue = hour.CloudCover,
                    Visibility = hour.Visibility.Value
                });

            }

            return hourly;
        }

        private IList<WeatherClass> ConvertDailyToWeatherClass(IList<DailyForecast> list)
        {

            daily = new List<WeatherClass>();
            foreach (var day in list)
            {
                daily.Add(new WeatherClass
                {
                    Date = day.Date,
                    TempMax = day.RealFeelTemperatureShade.Maximum.Value,
                    TempMin = day.RealFeelTemperatureShade.Minimum.Value,
                    WindDirection = day.Day.Wind.Direction.Degrees,
                    WindDirectionString = day.Day.Wind.Direction.Localized,
                    WindSpeed = day.Day.Wind.Speed.Value,
                    WeatherCode = day.Day.Icon,
                    PrecipProbability = day.Day.PrecipitationProbability,
                    Clouds = day.Day.ShortPhrase,
                    CloudsValue = day.Day.CloudCover,
                    Visibility = 100
                });

            }

            return daily;



        }
        int ChangeCode(int inputCode)
        {

            switch (inputCode)
            {
                case 1:
                    return 800;
                case 2:
                    return 800;
                case 3:
                    return 801;
                case 4:
                    return 801;
                case 5:
                    return 801;
                case 6:
                    return 802;
                case 7:
                    return 803;
                case 8:
                    return 804;
                case 11:
                    return 741;
                case 12:
                    return 502;
                case 13:
                    return 501;
                case 14:
                    return 501;
                case 15:
                    return 201;
                case 16:
                    return 202;
                case 17:
                    return 200;
                case 18:
                    return 501;
                case 19:
                    return 623;
                case 20:
                    return 623;
                case 21:
                    return 623;
                case 22:
                    return 621;
                case 23:
                    return 601;
                case 24:
                    return 601;
                case 25:
                    return 611;
                case 26:
                    return 511;
                case 29:
                    return 610;
                case 30:
                    return 1;
                case 31:
                    return 1;
                case 32:
                    return 700;
                case 33:
                    return 800;
                case 34:
                    return 800;
                case 35:
                    return 801;
                case 36:
                    return 801;
                case 37:
                    return 801;
                case 38:
                    return 803;
                case 39:
                    return 502;
                case 40:
                    return 501;
                case 41:
                    return 201;
                case 42:
                    return 202;
                case 43:
                    return 623;
                case 44:
                    return 601;

                default:
                    return 1;
            }
        }// This method changes the weather code to a common code.
    }
}
