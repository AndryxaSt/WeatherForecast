using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WeatherForecast
{
    class WeatherClass
    {
        public DateTime Date { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public double WindDirection { get; set; }
        public string WindDirectionString { get; set; }
        public double WindSpeed { get; set; }
        public int WeatherCode { get; set; }
        public double PrecipProbability { get; set; }
        public string Clouds { get; set; }
        public double CloudsValue { get; set; }
        public double Visibility { get; set; }
        public string  icon { get; set; }


        public WeatherClass()
        {

        }
        public WeatherClass(DateTime date, double tempMax, double tempMin, double windDirection, double windSpeed, int weatherCode, double precipProbability, string clouds, double cloudsValue,
            double visibility)
        {
            this.Date = date;
            this.TempMax = tempMax;
            this.TempMin = tempMin;
            this.WindDirection = windDirection;
            this.WindSpeed = windSpeed;
            this.WeatherCode = weatherCode;
            this.PrecipProbability = precipProbability;
            this.Clouds = clouds;
            this.CloudsValue = cloudsValue;
            this.Visibility = visibility;
        }
        public WeatherClass(DateTime date, double tempMax, double tempMin, double windDirection, double windSpeed, int weatherCode, string clouds, double cloudsValue)
        {
            this.Date = date;
            this.TempMax = tempMax;
            this.TempMin = tempMin;
            this.WindDirection = windDirection;
            this.WindSpeed = windSpeed;
            this.WeatherCode = weatherCode;
            this.Clouds = clouds;
            this.CloudsValue = cloudsValue;

        }
    }
}

