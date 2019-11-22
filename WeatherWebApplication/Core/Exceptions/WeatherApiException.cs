using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherWebApplication.Core.Exceptions
{
    public class WeatherApiException : Exception
    {
        public WeatherApiException(HttpResponseMessage response) : base(response.ReasonPhrase)
        {
            this.HResult = (int) response.StatusCode;
            this.Source = response.ReasonPhrase;
        }
    }
}
