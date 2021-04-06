using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Equinor.OmniaDataCatalogApi.Api.Controllers;
using Equinor.OmniaDataCatalogApi.Collibra.BusinessDataTypes;
using Equinor.OmniaDataCatalogApi.Collibra.Services;
using Equinor.OmniaDataCatalogApi.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Equinor.OmniaDataCatalogApi.UnitTests.Api
{
    public class CollibraControllerTests
    {
        private Mock<ILogger<CollibraController>> _loggerMock;
        private CollibraController _underTest;
        private Mock<ICollibraService> _collibraServiceMock;
        private const string bearer = "Bearer som-value-123";

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<CollibraController>>();
            _collibraServiceMock = new Mock<ICollibraService>();
            _underTest = new CollibraController(_loggerMock.Object, _collibraServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext {Request = {Headers = {{"Authorization", bearer}}}}
                }
            };
        }

        [Test]
        public async Task Get_Happy_Path_Should_Return_OkObjectResult()
        {
            // Arrange
            _collibraServiceMock.Setup(x => x.Communities(bearer, default))
                .ReturnsAsync(new List<Community> {new Community()});
            
            // Act
            var result = await _underTest.Communities();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_Happy_Path_Should_Return_Communities_Value()
        {
            // Arrange
            _collibraServiceMock.Setup(x => x.Communities(bearer, default))
                .ReturnsAsync(new List<Community> { new Community() });

            // Act
            var result = await _underTest.Communities() as OkObjectResult;

            //Assert
            Assert.IsInstanceOf<IEnumerable<Community>>(result?.Value);
        }

        
        [TestCase(typeof(HttpException), new Object[]{HttpStatusCode.Ambiguous}) ]
        [TestCase(typeof(Exception), new Object[]{})]
        public void Get_On_Exception_Throws_HttpException(Type exceptionType, Object[] exceptionArgs)
        {
            //Arrange
            var exceptionCtor = exceptionType.GetConstructors().ElementAtOrDefault(0);
            var exception = exceptionCtor?.Invoke(exceptionArgs);
            _collibraServiceMock.Setup(x => x.Communities(bearer, default))
                .Throws(exception as Exception);

            //Act & Assert
             Assert.ThrowsAsync<HttpException>( async ()=> await _underTest.Communities());
        }

        [TestCase(typeof(HttpException), new Object[] { HttpStatusCode.Ambiguous })]
        [TestCase(typeof(Exception), new Object[] { })]
        public void Get_On_Exception_Logs_Error(Type exceptionType, Object[] exceptionArgs)
        {
            //Arrange
            var exceptionCtor = exceptionType.GetConstructors().ElementAtOrDefault(0);
            var exception = exceptionCtor?.Invoke(exceptionArgs);
            _collibraServiceMock.Setup(x => x.Communities(bearer, default))
                .Throws(exception as Exception);

            //Act
            Assert.ThrowsAsync<HttpException>(async () => await _underTest.Communities());

            // Assert
            _loggerMock.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))
                , Times.Once);
        }
    }
}
