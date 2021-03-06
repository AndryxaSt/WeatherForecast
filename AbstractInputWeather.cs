﻿using System;
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
        protected Coordinates coordinates { get; set; }

        public AbstractInputWeather(string token, string location, Coordinates coordinates)
        {
            this.coordinates = coordinates;
            this.token = token;
            this.location = location;
        }
        public AbstractInputWeather(string token)
        {
            this.token = token;
        }

        protected IList<WeatherClass> hourly { get; set; }
        protected IList<WeatherClass> threeHourly { get; set; }
        protected IList<WeatherClass> daily { get; set; }
        //abstract public Task<IList<WeatherClass>> GetWeather();


    }
}
