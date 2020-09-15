using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
    struct Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public Coordinates(string coordinats)
        {
            Latitude = default;
            Longitude = default;
            SplitAndConvertCoordinats(coordinats);
        }
        public Coordinates(double lat, double lon)
        {
            this.Latitude = lat;
            this.Longitude = lon;
        }

        public void SplitAndConvertCoordinats(string coordinats)
        {
            string[] splitCoordinats = coordinats.Split(',');
            if (splitCoordinats.Length > 2)
            {
               Latitude = Convert.ToDouble(splitCoordinats[0] + "." + splitCoordinats[1]);
                Longitude = Convert.ToDouble(splitCoordinats[2] + "." + splitCoordinats[3]);
            }
            else 
            {
                Latitude = Convert.ToDouble(splitCoordinats[0]);
                Longitude = Convert.ToDouble(splitCoordinats[1]);
            }

        }

    }
}
