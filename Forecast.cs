using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        List<WeatherClass> weatherBits;
        List<WeatherClass>[] darkSkys;
        List<WeatherClass> openWeathers;
        List<WeatherClass>[] accuWeathers;

        Dictionary<string, string> tokens;
        public Forecast()
        {
            tokens = GetTokens();
            openWeather = new OpenWeather(tokens["OpenWeather"]);
            darkSky = new DarkSky(tokens["DarkSky"]);
            weatherBit = new WeatherBit(tokens["WeatherBit"]);
            accuWeather = new AccuWeather(tokens["AccuWeather"]);
        }


        private void GetWeather()

        {
            timeNow = DateTime.Now;

            if (weatherBits == null || weatherBits[0].Date.Day < timeNow.Day)
            {
                weatherBits = weatherBit.GetDaily();
            }
            if (openWeathers == null || openWeathers[0].Date.Day < timeNow.Day)
            {
                openWeathers = openWeather.GetWeather().Result;
            }
            if (darkSkys == null || darkSkys[0][0].Date.Day < timeNow.Day)
            {
                darkSkys = darkSky.GetFullWeather();
            }
            if (accuWeathers == null || accuWeathers[0][0].Date.Day < timeNow.Day)
            {
                accuWeathers = accuWeather.GetFullWeather();
            }

        }

        public void Test()
        {
            GetHourlyForecast();
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
                                      TempMax = (d.TempMax + w.TempMax + a.TempMax) / 3,
                                      TempMin = (d.TempMin + w.TempMin + a.TempMin) / 3,
                                      WindDirection = (d.WindDirection + w.WindDirection + a.WindDirection) / 3,
                                      WindDirectionString = ConvertBearingToDirection((d.WindDirection + w.WindDirection + a.WindDirection) / 3),
                                      WindSpeed = (d.WindSpeed + w.WindSpeed + a.WindSpeed) / 3,
                                      WeatherCode = 0,//Заглушка
                                      PrecipProbability = (d.PrecipProbability + w.PrecipProbability + a.PrecipProbability) / 3,
                                      Clouds = w.Clouds,
                                      CloudsValue = (d.CloudsValue + w.CloudsValue + a.CloudsValue) / 3,
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
                                            TempMax = (d.TempMax + o.TempMax) / 2,
                                            TempMin = (d.TempMin + o.TempMin) / 2,
                                            WindDirection = (d.WindDirection + o.WindDirection) / 2,
                                            WindDirectionString = ConvertBearingToDirection((d.WindDirection + o.WindDirection) / 2),
                                            WindSpeed = (d.WindSpeed + o.WindSpeed) / 2,
                                            WeatherCode = 0,//Заглушка
                                            PrecipProbability = 0,//Заглушка
                                            Clouds = o.Clouds,
                                            CloudsValue = (d.CloudsValue + o.CloudsValue) / 2,
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
                                       TempMax = (d.TempMax + a.TempMax) / 2,
                                       TempMin = (d.TempMin + a.TempMin) / 2,
                                       WindDirection = (d.WindDirection + a.WindDirection) / 2,
                                       WindDirectionString = ConvertBearingToDirection((d.WindDirection + a.WindDirection) / 2),
                                       WindSpeed = (d.WindSpeed + a.WindSpeed) / 2,
                                       WeatherCode = 0,//Заглушка
                                       PrecipProbability = (d.PrecipProbability + a.PrecipProbability) / 2,
                                       Clouds = a.Clouds,
                                       CloudsValue = (d.CloudsValue + a.CloudsValue) / 2,
                                       Visibility = (d.Visibility + a.Visibility) / 2,
                                       icon = "A",//Заглушка
                                   };

            Display(midHourlyWeather);
        }

        private string ConvertToString(WeatherClass inpWeather)//TODO: Переделать на StringBuilder
        {

            string weather = $@"{inpWeather.Date.ToLongDateString()}{inpWeather.Date.ToShortTimeString()}
Температура: max:{inpWeather.TempMax} C
             min:{inpWeather.TempMin} C, 
Ветер: {ConvertBearingToDirection(inpWeather.WindDirection)} {inpWeather.WindSpeed} м\с, 
{inpWeather.Clouds}, облачность: {inpWeather.CloudsValue}%
Вероятность осадков: {inpWeather.PrecipProbability}% " + "\n";
            return weather;

        }
        private string ConvertBearingToDirection(double bearing)
        {
            double bearing2 = bearing;


            if (bearing2 <= 11.25)
            {
                return "С";
            }
            if (bearing2 > 11.25 && bearing2 <= 33.75)
            {
                return "ССВ";
            }
            if (bearing2 > 33.75 && bearing2 <= 56.25)
            {
                return "СВ";
            }
            if (bearing2 > 56.25 && bearing2 <= 78.75)
            {
                return "ВСВ";
            }
            if (bearing2 > 78.75 && bearing2 <= 101.25)
            {
                return "В";
            }
            if (bearing2 > 101.25 && bearing2 <= 123.75)
            {
                return "ВЮВ";
            }
            if (bearing2 > 123.75 && bearing2 <= 146.25)
            {
                return "ЮВ";
            }
            if (bearing2 > 146.25 && bearing2 <= 168.75)
            {
                return "ЮЮВ";
            }
            if (bearing2 > 168.75 && bearing2 <= 191.25)
            {
                return "Ю";
            }
            if (bearing2 > 191.25 && bearing2 <= 213.75)
            {
                return "ЮЮЗ";
            }
            if (bearing2 > 213.75 && bearing2 <= 236.25)
            {
                return "ЮЗ";
            }
            if (bearing2 > 236.25 && bearing2 <= 258.75)
            {
                return "ЗЮЗ";
            }
            if (bearing2 > 258.75 && bearing2 <= 281.25)
            {
                return "З";
            }
            if (bearing2 > 281.25 && bearing2 <= 303.75)
            {
                return "ЗСЗ";
            }
            if (bearing2 > 303.75 && bearing2 <= 326.25)
            {
                return "СЗ";
            }
            if (bearing2 > 326.25 && bearing2 <= 348.75)
            {
                return "ССЗ";
            }
            if (bearing2 > 326.25)
            {
                return "С";
            }

            return string.Empty;

        }
        private void Display(IEnumerable<WeatherClass> weathers)
        {
            foreach (WeatherClass weather in weathers)
            {
                Console.WriteLine(ConvertToString(weather));
            }
        }
        private Dictionary<string, string> GetTokens()
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
    }
}
