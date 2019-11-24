using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using WeatherWebApplication.Models;

namespace WeatherWebApplication.Core
{
    public class AccuWeatherApi : WeatherApi
    {
        
        public AccuWeatherApi() : base()
        {
            
            this.WeatherServiceName = "AccuWeather";
            this.Token = "A1lsqa8gB6LC3N58eWhrOQA3U4zeLUZ4";
            this.Url = new Uri("http://dataservice.accuweather.com");
            
            
        }

        public override async Task<Forecast> GetForecastOnDay(City city)
        {
            if (city == null)
                throw new Exception("City is not be null");
            
            Forecast forecast = new Forecast();
            
            var p = GetCityNumber(city);
            p.Wait();
            

            if (p.IsCompletedSuccessfully && !String.IsNullOrEmpty(p.Result))
            {

                string cityLocationNumber = p.Result;

                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response =
                        await client.GetAsync(GetForecastUri(cityLocationNumber)))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var reponseJson = await response.Content.ReadAsStringAsync();
                            //Преобразуем JSON из респонса в JArray, берем первый объект и обращаемся к его ключам как к полям класса
                            var responseArray = JArray.Parse(reponseJson);
                            dynamic jobject = (JObject) responseArray.FirstOrDefault();

                            forecast.Temp = (float) jobject.Temperature.Metric.Value;
                            forecast.Humidity = (float) jobject.RelativeHumidity;
                            forecast.Pressure = (float) jobject.Pressure.Metric.Value;

                        }
                        else
                        {
                            throw new Exceptions.WeatherApiException(response);
                        }
                    }
                }
            }
            forecast.WeatherApi = this;
            forecast.city = city;
            return forecast;
        }

        private Uri GetForecastUri(string cityNumber)
        {
            
            return new Uri($"{this.Url}/currentconditions/v1/{cityNumber}?apikey={this.Token}&details=true");
            
        }

        private Uri GetAccuWeatherCityNumberUri(City city)
        {
            if (city == null)
                throw new Exception("City is not be null");

            return new Uri($"{this.Url}/locations/v1/cities/search?apikey={this.Token}&q={city.Name}&offset=1");
        }

        
        public async Task<string> GetCityNumber(City city)
        {
            if (city == null)
                throw new Exception("City is not be null");

            string cityNumber = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(GetAccuWeatherCityNumberUri(city)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var reponseJson = await response.Content.ReadAsStringAsync();
                        var responseArray = JArray.Parse(reponseJson);
                        JObject jobject = (JObject)responseArray.FirstOrDefault();

                        cityNumber = jobject.GetValue("Key").ToString();
                    }
                    else
                    {
                        this.ServiceAvailable = false;
                    }
                }
            }

            return cityNumber;
        }

        
    }
}
