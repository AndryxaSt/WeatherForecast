using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WeatherForecast
{
    public class WeatherClass
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
        public string icon { get; set; }


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



        public override string ToString()
        {
            return $@"{Date.ToLongDateString()}{Date.ToShortTimeString()}
Температура: max:{TempMax} C
             min:{TempMin} C, 
Ветер: {ConvertBearingToDirection(WindDirection)} {WindSpeed} м\с, 
{Clouds}, облачность: {CloudsValue}%
Вероятность осадков: {PrecipProbability}% " + "\n";

        }
        static string ConvertBearingToDirection(double inputBearing)
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

    }
}

