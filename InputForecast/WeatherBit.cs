using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
     class WeatherBit//https://www.weatherbit.io
    {
        List<WeatherClass> daily;
        readonly string token;

        public WeatherBit(string token)
        {
            this.token = token;
        }
        #region JSON Classes
        class Weather
        {
            public string icon { get; set; }
            public int code { get; set; }
            public string description { get; set; }
        }

        class Datum
        {
            public int moonrise_ts { get; set; }
            public string wind_cdir { get; set; }
            public int rh { get; set; }
            public double pres { get; set; }
            public double high_temp { get; set; }
            public int sunset_ts { get; set; }
            public double ozone { get; set; }
            public double moon_phase { get; set; }
            public double wind_gust_spd { get; set; }
            public double snow_depth { get; set; }
            public int clouds { get; set; }
            public int ts { get; set; }
            public int sunrise_ts { get; set; }
            public double app_min_temp { get; set; }
            public double wind_spd { get; set; }
            public int pop { get; set; }
            public string wind_cdir_full { get; set; }
            public double slp { get; set; }
            public string valid_date { get; set; }
            public double app_max_temp { get; set; }
            public double vis { get; set; }
            public double dewpt { get; set; }
            public double snow { get; set; }
            public double uv { get; set; }
            public Weather weather { get; set; }
            public int wind_dir { get; set; }
            public object max_dhi { get; set; }
            public int clouds_hi { get; set; }
            public double precip { get; set; }
            public double low_temp { get; set; }
            public double max_temp { get; set; }
            public int moonset_ts { get; set; }
            public string datetime { get; set; }
            public double temp { get; set; }
            public double min_temp { get; set; }
            public int clouds_mid { get; set; }
            public int clouds_low { get; set; }
        }

        class RootObject
        {
            public List<Datum> data { get; set; }
            public string city_name { get; set; }
            public string lon { get; set; }
            public string timezone { get; set; }
            public string lat { get; set; }
            public string country_code { get; set; }
            public string state_code { get; set; }
        }
        #endregion

        private static double ConvertDirectionToBearing(string directionInput)
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

        private RootObject GetWeather()
        {
            WebRequest requestBit = WebRequest.Create(@"https://api.weatherbit.io/v2.0/forecast/daily?city=Konotop&country=ua&days=6&units=M&lang=ru&key="+token);

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

        public List<WeatherClass> GetDaily()
        {
            daily = ConvertToWeatherClass(GetWeather());

            return daily;
        }
        private  List<WeatherClass> ConvertToWeatherClass(RootObject rootObject)
        {
            daily = new List<WeatherClass>();


            foreach (var item in rootObject.data)
            {
                daily.Add(new WeatherClass
                    
                {
                    Date = Convert.ToDateTime(item.valid_date),
                    TempMax = item.max_temp,
                    TempMin = item.min_temp,
                    WindDirection = ConvertDirectionToBearing(item.wind_cdir),
                    WindSpeed = item.wind_spd,
                    WeatherCode = item.weather.code,
                    PrecipProbability = item.pop,
                    Clouds = item.weather.description,
                    CloudsValue = item.clouds,
                    Visibility = item.vis
                });
            }

            return daily;
        }
    }
}
