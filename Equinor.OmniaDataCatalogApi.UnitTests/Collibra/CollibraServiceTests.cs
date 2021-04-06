using System.Collections.Generic;
using System.Threading.Tasks;
using Equinor.OmniaDataCatalogApi.Collibra.BusinessDataTypes;
using Equinor.OmniaDataCatalogApi.Collibra.Converters;
using Equinor.OmniaDataCatalogApi.Collibra.Dtos;
using Equinor.OmniaDataCatalogApi.Collibra.HttpClients;
using Equinor.OmniaDataCatalogApi.Collibra.Services;
using Moq;
using NUnit.Framework;

namespace Equinor.OmniaDataCatalogApi.UnitTests.Collibra
{
    public class CollibraServiceTests
    {
        private Mock<ICollibraHttpClient> _collibraHttpClientMock;
        private Mock<ICommunitiesConverter> _communitiesConverterMock;
        private CollibraService _underTest;

        private const string authorizationHeaderValue = "Bearer something-647867438nhjitnbhji7787UYT";

        [SetUp]
        public void Setup()
        {
            _collibraHttpClientMock = new Mock<ICollibraHttpClient>();
            _communitiesConverterMock = new Mock<ICommunitiesConverter>();
            _underTest = new CollibraService(_collibraHttpClientMock.Object, _communitiesConverterMock.Object);
        }

        [Test]
        public async Task Should_Add_Authorization_Header_To_Collibra_Call()
        {
            //Arrange

            //Act
            await _underTest.Communities(authorizationHeaderValue);

            //Assert
            _collibraHttpClientMock.Verify(
                x => x.Get("rest/2.0/communities",
                    It.Is<Dictionary<string, string>>(x => x["Authorization"] == authorizationHeaderValue), default),
                Times.Once);
        }

        [Test]
        public async Task Should_Convert_Collibra_Values_To_Business_Type()
        {
            //Arrange
            var communitiesDto = new CommunitiesResultDto {Communities = new[] {new CommunityDto()}};
            _collibraHttpClientMock.Setup(
                    x => x.Get("rest/2.0/communities",
                        It.Is<Dictionary<string, string>>(x => x["Authorization"] == authorizationHeaderValue),
                        default))
                .ReturnsAsync(communitiesDto);

            //Act
            await _underTest.Communities(authorizationHeaderValue);

            //Assert
            _communitiesConverterMock.Verify(x => x.Convert(communitiesDto), Times.Once);
        }

        [Test]
        public async Task Should_Return_Converter_Result()
        {
            //Arrange
            var communitiesDto = new CommunitiesResultDto { Communities = new[] { new CommunityDto() } };
            var resultCommunities = new[] { new Community{Id = "13", Name = "Hannah Smith"}};
            _collibraHttpClientMock.Setup(
                    x => x.Get("rest/2.0/communities",
                        It.Is<Dictionary<string, string>>(x => x["Authorization"] == authorizationHeaderValue),
                        default))
                .ReturnsAsync(communitiesDto);

            _communitiesConverterMock.Setup(x => x.Convert(communitiesDto)).Returns(resultCommunities);

            //Act
            var result  = await _underTest.Communities(authorizationHeaderValue);

            //Assert
            Assert.AreEqual(resultCommunities, result);
        }
    }
}