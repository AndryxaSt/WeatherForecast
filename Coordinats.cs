using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
    struct Coordinats
    {
        public double lat { get; set; }
        public double lon { get; set; }

        public Coordinats(double lat,double lon)
        {
            this.lat = lat;
            this.lon = lon;
        }

    }
}
