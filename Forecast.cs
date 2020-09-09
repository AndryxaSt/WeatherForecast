using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;


namespace WeatherForecast
{

    public class Forecast
    {
        DarkSky darkSky;//1,3hourly, daily
        OpenWeather openWeather;//3hourly
        WeatherBit weatherBit; //daily
        AccuWeather accuWeather; //12hourly, 5daily

        DateTime timeNow;
        string location;

        IList<WeatherClass> weatherBits, openWeathers;
        IList<WeatherClass>[] darkSkys, accuWeathers;

        Dictionary<string, string> tokens;
        public Forecast(string location = "47.8229, 35.1903")
        {
            tokens = GetTokens();
            openWeather = new OpenWeather(tokens["OpenWeather"], location);
            darkSky = new DarkSky(tokens["DarkSky"], location);
            weatherBit = new WeatherBit(tokens["WeatherBit"], location);
            accuWeather = new AccuWeather(tokens["AccuWeather"], new Coordinats(Convert.ToDouble(location.Split(',')[0]), Convert.ToDouble(location.Split(',')[1])));//TODO: Исправить получение погоды по координатам (https://www.accuweather.com/ru/search-locations?query=54.9924%2C+73.3686)
            this.location = location;
        }


        void GetWeather()

        {
            timeNow = DateTime.Now;

            if (weatherBits == null || weatherBits[0].Date.Day < timeNow.Day)
            {
                weatherBits = weatherBit.GetWeather();
            }
            if (openWeathers == null || openWeathers[0].Date.Day < timeNow.Day)
            {
                openWeathers = openWeather.GetWeatherAsync().Result;
            }
            if (darkSkys == null || darkSkys[0][0].Date.Day < timeNow.Day)
            {
                darkSkys = darkSky.GetWeather();
            }
            if (accuWeathers == null || accuWeathers[0][0].Date.Day < timeNow.Day)
            {
                accuWeathers = accuWeather.GetWeather();
            }

        }

        public void Test()
        {
            GetThreeHourlyForecast();
        }
        public void GetDailyForecast()
        {
            GetWeather();

            var midDailyWeather = from d in darkSkys[2]
                                  from w in weatherBits
                                  from a in accuWeathers[1]
                                  where w.Date.ToShortDateString() == a.Date.ToShortDateString() && w.Date.ToShortDateString() == d.Date.ToShortDateString()
                                  select new WeatherClass()
                                  {
                                      Date = d.Date,
                                      TempMax = Math.Round((d.TempMax + w.TempMax + a.TempMax) / 3, 1),
                                      TempMin = Math.Round((d.TempMin + w.TempMin + a.TempMin) / 3, 1),
                                      WindDirection = Math.Round((d.WindDirection + w.WindDirection + a.WindDirection) / 3, 1),
                                      WindDirectionString = ConvertBearingToDirection((d.WindDirection + w.WindDirection + a.WindDirection) / 3),
                                      WindSpeed = Math.Round((d.WindSpeed + w.WindSpeed + a.WindSpeed) / 3, 1),
                                      WeatherCode = WeatherCodeComparison(new int[3] { a.WeatherCode, d.WeatherCode, w.WeatherCode }),
                                      PrecipProbability = Math.Round((d.PrecipProbability + w.PrecipProbability + a.PrecipProbability) / 3, 1),
                                      Clouds = w.Clouds,
                                      CloudsValue = Math.Round((d.CloudsValue + w.CloudsValue + a.CloudsValue) / 3, 1),
                                      Visibility = (d.Visibility + w.Visibility + a.Visibility) / 3,
                                      icon = "A",//Заглушка

                                  };

            Display(midDailyWeather);

        }
        public void GetThreeHourlyForecast()
        {
            GetWeather();

            var midThreeHourlyWeather = from d in darkSkys[1]
                                        from o in openWeathers
                                        where d.Date == o.Date
                                        select new WeatherClass()
                                        {
                                            Date = d.Date,
                                            TempMax = Math.Round((d.TempMax + o.TempMax) / 2, 1),
                                            TempMin = Math.Round((d.TempMin + o.TempMin) / 2, 1),
                                            WindDirection = Math.Round((d.WindDirection + o.WindDirection) / 2, 1),
                                            WindDirectionString = ConvertBearingToDirection((d.WindDirection + o.WindDirection) / 2),
                                            WindSpeed = Math.Round((d.WindSpeed + o.WindSpeed) / 2, 1),
                                            WeatherCode = WeatherCodeComparison(new int[2] { o.WeatherCode, d.WeatherCode }),
                                            PrecipProbability = 0,//Заглушка
                                            Clouds = o.Clouds,
                                            CloudsValue = Math.Round((d.CloudsValue + o.CloudsValue) / 2, 1),
                                            Visibility = 0,//Заглушка
                                            icon = "A",//Заглушка

                                        };

            Display(midThreeHourlyWeather);

        }
        public void GetHourlyForecast()
        {
            GetWeather();
            var midHourlyWeather = from a in accuWeathers[0]
                                   from d in darkSkys[0]
                                   where a.Date == d.Date
                                   select new WeatherClass()
                                   {
                                       Date = d.Date,
                                       TempMax = Math.Round((d.TempMax + a.TempMax) / 2, 1),
                                       TempMin = Math.Round((d.TempMin + a.TempMin) / 2, 1),
                                       WindDirection = Math.Round((d.WindDirection + a.WindDirection) / 2, 1),
                                       WindDirectionString = ConvertBearingToDirection((d.WindDirection + a.WindDirection) / 2),
                                       WindSpeed = Math.Round((d.WindSpeed + a.WindSpeed) / 2, 1),
                                       WeatherCode = WeatherCodeComparison(new int[2] { a.WeatherCode, d.WeatherCode }),
                                       PrecipProbability = Math.Round((d.PrecipProbability + a.PrecipProbability) / 2, 1),
                                       Clouds = a.Clouds,
                                       CloudsValue = Math.Round((d.CloudsValue + a.CloudsValue) / 2, 1),
                                       Visibility = Math.Round((d.Visibility + a.Visibility) / 2, 1),
                                       icon = "A",//Заглушка
                                   };

            Display(midHourlyWeather);
        }



        string ConvertBearingToDirection(double inputBearing)
        {
            double outputBearing = inputBearing;


            if (outputBearing <= 11.25)
            {
                return "С";
            }
            if (outputBearing > 11.25 && outputBearing <= 33.75)
            {
                return "ССВ";
            }
            if (outputBearing > 33.75 && outputBearing <= 56.25)
            {
                return "СВ";
            }
            if (outputBearing > 56.25 && outputBearing <= 78.75)
            {
                return "ВСВ";
            }
            if (outputBearing > 78.75 && outputBearing <= 101.25)
            {
                return "В";
            }
            if (outputBearing > 101.25 && outputBearing <= 123.75)
            {
                return "ВЮВ";
            }
            if (outputBearing > 123.75 && outputBearing <= 146.25)
            {
                return "ЮВ";
            }
            if (outputBearing > 146.25 && outputBearing <= 168.75)
            {
                return "ЮЮВ";
            }
            if (outputBearing > 168.75 && outputBearing <= 191.25)
            {
                return "Ю";
            }
            if (outputBearing > 191.25 && outputBearing <= 213.75)
            {
                return "ЮЮЗ";
            }
            if (outputBearing > 213.75 && outputBearing <= 236.25)
            {
                return "ЮЗ";
            }
            if (outputBearing > 236.25 && outputBearing <= 258.75)
            {
                return "ЗЮЗ";
            }
            if (outputBearing > 258.75 && outputBearing <= 281.25)
            {
                return "З";
            }
            if (outputBearing > 281.25 && outputBearing <= 303.75)
            {
                return "ЗСЗ";
            }
            if (outputBearing > 303.75 && outputBearing <= 326.25)
            {
                return "СЗ";
            }
            if (outputBearing > 326.25 && outputBearing <= 348.75)
            {
                return "ССЗ";
            }
            if (outputBearing > 326.25)
            {
                return "С";
            }

            return string.Empty;

        }
        void Display(IEnumerable<WeatherClass> forecast)
        {
            foreach (WeatherClass weather in forecast)
            {
                Console.WriteLine(weather);
            }
        }
        Dictionary<string, string> GetTokens()
        {
            string[] tokenFile = File.ReadAllLines(@"Tokens.txt");
            var tokens = new Dictionary<string, string>();
            string[] temp;
            for (int i = 0; i < tokenFile.Length; i++)
            {
                temp = tokenFile[i].Split(':');
                tokens.Add(temp[0].Trim(), temp[1].Trim());
            }

            return tokens;
        }
        int WeatherCodeComparison(int[] codes)
        {


            return 1;
        }


    }
}
