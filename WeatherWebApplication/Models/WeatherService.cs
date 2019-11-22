using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherWebApplication.Models
{
    public class WeatherService
    {
        public Core.WeatherApi WeatherApi { get; set; }

        public bool Selected { get; set; }
    }
}
