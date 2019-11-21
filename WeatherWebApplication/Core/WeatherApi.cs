using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherWebApplication.Core
{
    public abstract class WeatherApi
    {
        public string Uri { get; set; }

        public string WeatherServiceName { get; set; }

        protected string Token { get; set; }

        public abstract Task<Models.Forecast> GetForecastOnDay(Models.City city);

    }
}
