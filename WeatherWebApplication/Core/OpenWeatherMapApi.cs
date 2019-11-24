using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using WeatherWebApplication.Models;

namespace WeatherWebApplication.Core
{
    public class OpenWeatherMapApi : WeatherApi
    {
        public OpenWeatherMapApi() : base()
        {
            this.WeatherServiceName = "OpenWeatherMap";
            this.Token = "9b70f4ad5c7d79452694701a8d81c1b3";
            this.Url = new Uri("http://api.openweathermap.org/");
        }

        public override async Task<Forecast> GetForecastOnDay(Models.City city)
        {
            if (city == null)
                throw new Exception("City is not be null");
            var forecast = new Models.Forecast();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (HttpResponseMessage response =
                        await client.GetAsync(GetForecastUri(city.Name)))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                             
                            var responseJson = await response.Content.ReadAsStringAsync();
                            //Преобразуем JSON из респонса в JObject, и обращаемся к его ключам как к полям класса,
                            dynamic responseObj = Newtonsoft.Json.Linq.JObject.Parse(responseJson);
                            forecast.Temp = (float)responseObj.main.temp;
                            forecast.Humidity = (float)responseObj.main.humidity;
                            forecast.Pressure = (float)responseObj.main.pressure;
                        }
                        else
                        {
                            Console.WriteLine(response.ReasonPhrase);
                            this.ServiceAvailable = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    this.ServiceAvailable = false;
                }
                
            }
            forecast.city = city;
            forecast.WeatherApi = this;
            return forecast;
        }

        private Uri GetForecastUri(string cityName)
        {
            return new Uri($"{this.Url}/data/2.5/weather?q={cityName}&units=metric&appid={this.Token}");
        }
    }
}
