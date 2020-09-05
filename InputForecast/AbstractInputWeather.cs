using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.InputForecast
{
    abstract class AbstractInputWeather
    {
        protected string token { get; set; }
        protected string location { get; set; }

        public AbstractInputWeather(string token, string location)
        {
            this.token = token;
            this.location = location;
        }

        protected IList<WeatherClass> hourly { get;  set; }
        protected IList<WeatherClass> threeHourly { get;  set; }
        protected IList<WeatherClass> daily { get;  set; }
        //abstract public Task<IList<WeatherClass>> GetWeather();
       

    }
}
