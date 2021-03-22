using System.Collections.Generic;
using HelloRadix;
using HelloRadix.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace HelloRadixTests
{
    public class WeatherForecastControllerTests
    {
        private Mock<ILogger<WeatherForecastController>> _loggerMock;
        private WeatherForecastController _underTest;
        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<WeatherForecastController>>();
            _underTest = new WeatherForecastController(_loggerMock.Object);
        }

        [Test]
        public void Get_Should_Return_Expected_DataType()
        {
            var result = _underTest.Get();
            Assert.IsInstanceOf<IEnumerable<WeatherForecast>>(result);
        }

/*        [Test]
        public void WantThisToFail()
        {
            Assert.IsTrue(false);
        }*/
    }
}
