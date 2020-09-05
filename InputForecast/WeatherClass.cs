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
            double outputBearing = inputBearing;


            if (outputBearing <= 11.25)
            {
                return "С";
            }
            if (outputBearing > 11.25 && outputBearing <= 33.75)
            {
                return "ССВ";
            }
            if (outputBearing > 33.75 && outputBearing <= 56.25)
            {
                return "СВ";
            }
            if (outputBearing > 56.25 && outputBearing <= 78.75)
            {
                return "ВСВ";
            }
            if (outputBearing > 78.75 && outputBearing <= 101.25)
            {
                return "В";
            }
            if (outputBearing > 101.25 && outputBearing <= 123.75)
            {
                return "ВЮВ";
            }
            if (outputBearing > 123.75 && outputBearing <= 146.25)
            {
                return "ЮВ";
            }
            if (outputBearing > 146.25 && outputBearing <= 168.75)
            {
                return "ЮЮВ";
            }
            if (outputBearing > 168.75 && outputBearing <= 191.25)
            {
                return "Ю";
            }
            if (outputBearing > 191.25 && outputBearing <= 213.75)
            {
                return "ЮЮЗ";
            }
            if (outputBearing > 213.75 && outputBearing <= 236.25)
            {
                return "ЮЗ";
            }
            if (outputBearing > 236.25 && outputBearing <= 258.75)
            {
                return "ЗЮЗ";
            }
            if (outputBearing > 258.75 && outputBearing <= 281.25)
            {
                return "З";
            }
            if (outputBearing > 281.25 && outputBearing <= 303.75)
            {
                return "ЗСЗ";
            }
            if (outputBearing > 303.75 && outputBearing <= 326.25)
            {
                return "СЗ";
            }
            if (outputBearing > 326.25 && outputBearing <= 348.75)
            {
                return "ССЗ";
            }
            if (outputBearing > 326.25)
            {
                return "С";
            }

            return string.Empty;

        }

    }
}

