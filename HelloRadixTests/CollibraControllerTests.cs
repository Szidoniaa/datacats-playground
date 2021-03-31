using System.Collections.Generic;
using System.Threading.Tasks;
using HelloRadix.BusinessDataTypes;
using HelloRadix.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace HelloRadixTests
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
        public async Task Get_Should_Return_Expected_DataType()
        {
            // Arrange
            _collibraServiceMock.Setup(x => x.Communities(bearer, default))
                .ReturnsAsync(new List<Community> {new Community()});
            
            // Act
            var result = await _underTest.Communities();

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

/*        [Test]
        public void WantThisToFail()
        {
            Assert.IsTrue(false);
        }*/
    }
}
