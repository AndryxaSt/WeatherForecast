using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.InputForecast
{
    abstract class AbdtractInputWeather
    {
        protected string token { get; set; }
        protected string location { get; set; }

        public AbdtractInputWeather(string token, string location)
        {
            this.token = token;
            this.location = location;
        }

        protected IList<WeatherClass> hourly;
        protected IList<WeatherClass> threeHourly;
        protected IList<WeatherClass> daily;


    }
}
