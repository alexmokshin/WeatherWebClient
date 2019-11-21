using NUnit.Framework;
using WeatherWebApplication.Core;
using WeatherWebApplication.Models;
using WeatherWebApplication.Pages;
using System;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            City city = new City();
            city.Name = "London";
            WeatherApi api = new OpenWeatherMapApi();
            var response = api.GetForecastOnDay(city).Result;
            Assert.IsNotNull(response);
            Assert.NotZero(response.Temp);
            Assert.NotZero(response.Humidity);
            Assert.NotZero(response.Pressure);


        }
    }
}