using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherWebApplication.Models
{
    public class Forecast
    {
        
        public float Pressure { get; set; }
        public float Humidity { get; set; }
        public float Temp { get; set; }
        public City city { get; set; }
    }
}
