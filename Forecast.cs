using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace WeatherForecast
{

    public class Forecast
    {
        Stopwatch stopwatch = new Stopwatch();

        DarkSky darkSky;//1,3hourly, daily
        OpenWeather openWeather;//3hourly
        WeatherBit weatherBit; //daily
        AccuWeather accuWeather; //12hourly, 5daily

        DateTime timeNow;
        string location;
        Coordinates coordinates;

        IList<WeatherClass> weatherBits, openWeathers;
        IList<WeatherClass>[] darkSkys, accuWeathers;

        Dictionary<string, string> tokens;

        public Forecast(string location = "49.9808, 36.2527")
        {
            this.location = location;
            coordinates = new Coordinates(location);
            tokens = GetTokens();
            openWeather = new OpenWeather(tokens["OpenWeather"], location, coordinates);
            darkSky = new DarkSky(tokens["DarkSky"], location, coordinates);
            weatherBit = new WeatherBit(tokens["WeatherBit"], location, coordinates);
            accuWeather = new AccuWeather(tokens["AccuWeather"], location, coordinates);

        }

        void GetWeather()

        {
            timeNow = DateTime.Now;
            try
            {
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

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        async Task GetWeatherAsync()

        {
            timeNow = DateTime.Now;
            try
            {
                Task<IList<WeatherClass>> weatherBitsTask = weatherBit.GetWeatherAsync();
                Task<IList<WeatherClass>> openWeathersTask = openWeather.GetWeatherAsync();
                Task<IList<WeatherClass>[]> darkSkysTask = darkSky.GetWeatherAsync();
                Task<IList<WeatherClass>[]> accuWeathersTask = accuWeather.GetWeatherAsync();

                weatherBits = weatherBitsTask.Result;
                openWeathers = openWeathersTask.Result;
                darkSkys = darkSkysTask.Result;
                accuWeathers = accuWeathersTask.Result;

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public IEnumerable<WeatherClass> GetDailyForecast()
        {
            GetWeatherAsync().Wait();

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

            return midDailyWeather;

        }
        public IEnumerable<WeatherClass> GetThreeHourlyForecast()
        {
            GetWeatherAsync().Wait();

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
                                            PrecipProbability = Math.Round((d.PrecipProbability + o.PrecipProbability) / 2, 1),
                                            Clouds = o.Clouds,
                                            CloudsValue = Math.Round((d.CloudsValue + o.CloudsValue) / 2, 1),
                                            Visibility = (d.Visibility + o.Visibility) / 2,
                                            icon = "A",//Заглушка

                                        };

            return midThreeHourlyWeather;


        }
        public IEnumerable<WeatherClass> GetHourlyForecast()
        {
            GetWeatherAsync().Wait();
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

            return midHourlyWeather;
        }

        string ConvertBearingToDirection(double inputBearing)
        {
            if (inputBearing <= 11.25)
            {
                return "С";
            }
            if (inputBearing > 11.25 && inputBearing <= 33.75)
            {
                return "ССВ";
            }
            if (inputBearing > 33.75 && inputBearing <= 56.25)
            {
                return "СВ";
            }
            if (inputBearing > 56.25 && inputBearing <= 78.75)
            {
                return "ВСВ";
            }
            if (inputBearing > 78.75 && inputBearing <= 101.25)
            {
                return "В";
            }
            if (inputBearing > 101.25 && inputBearing <= 123.75)
            {
                return "ВЮВ";
            }
            if (inputBearing > 123.75 && inputBearing <= 146.25)
            {
                return "ЮВ";
            }
            if (inputBearing > 146.25 && inputBearing <= 168.75)
            {
                return "ЮЮВ";
            }
            if (inputBearing > 168.75 && inputBearing <= 191.25)
            {
                return "Ю";
            }
            if (inputBearing > 191.25 && inputBearing <= 213.75)
            {
                return "ЮЮЗ";
            }
            if (inputBearing > 213.75 && inputBearing <= 236.25)
            {
                return "ЮЗ";
            }
            if (inputBearing > 236.25 && inputBearing <= 258.75)
            {
                return "ЗЮЗ";
            }
            if (inputBearing > 258.75 && inputBearing <= 281.25)
            {
                return "З";
            }
            if (inputBearing > 281.25 && inputBearing <= 303.75)
            {
                return "ЗСЗ";
            }
            if (inputBearing > 303.75 && inputBearing <= 326.25)
            {
                return "СЗ";
            }
            if (inputBearing > 326.25 && inputBearing <= 348.75)
            {
                return "ССЗ";
            }
            if (inputBearing > 326.25)
            {
                return "С";
            }

            return string.Empty;

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
//async void GetWeather()

//{
//    timeNow = DateTime.Now;
//    try
//    {
//        if (weatherBits == null || weatherBits[0].Date.Day < timeNow.Day)
//        {
//            Task<IList<WeatherClass>> weatherBitsTask = weatherBit.GetWeatherAsync();
//        }
//        if (openWeathers == null || openWeathers[0].Date.Day < timeNow.Day)
//        {
//            Task<IList<WeatherClass>> openWeathersTask = openWeather.GetWeatherAsync();
//        }
//        if (darkSkys == null || darkSkys[0][0].Date.Day < timeNow.Day)
//        {
//            Task<IList<WeatherClass>[]> darkSkysTask = darkSky.GetWeatherAsync();
//        }
//        if (accuWeathers == null || accuWeathers[0][0].Date.Day < timeNow.Day)
//        {
//            Task<IList<WeatherClass>[]> accuWeathersTask = accuWeather.GetWeatherAsync();
//        }

//        weatherBits = weatherBitsTask.Result

//            }