using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Services;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Models.Records;
using CE_API_V2.UnitOfWorks.Interfaces;
using Moq;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class UserInputTemplateServiceTests
    {
        private IUserInputTemplateService _userInputTemplateService;
        private CreateUser _mockedCreateUser;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            var mapper = mapperConfig.CreateMapper();

            _mockedCreateUser = MockDataProvider.GetCreateUser();
            var userModel = mapper.Map<UserModel>(_mockedCreateUser);
            userModel.UserId = Guid.NewGuid().ToString();
            var billingMock = MockDataProvider.GetBillingMock();

            var userUowMock = new Mock<IUserUOW>();
            userUowMock.Setup(x => x.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(userModel);
            userUowMock.Setup(x => x.GetBilling(It.IsAny<string>())).Returns(billingMock);

            _userInputTemplateService = new UserInputTemplateService(mapper, userUowMock.Object);
        }

        [Test]
        public async Task GetTemplate_GivenCorrectLocalization_ExpectedCorrectlyAssembledUserInputTemplate()
        {
            //Arrange
            var userIdsRecord = new UserIdsRecord();

            //Act
            var result = await _userInputTemplateService.GetTemplate(userIdsRecord, "en-GB");

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserInputFormSchema>();
            result.City.Should().Be(_mockedCreateUser.City);
            result.CityHeader.Should().Be("City");
        }

        [Test]
        public async Task GetTemplate_GivenInCorrectLocalization_ExpectedCorrectlyAssembledUserInputTemplate()
        {
            //Arrange
            var userIdsRecord = new UserIdsRecord();
            var locale = "invalid";

            //Act
            var result = await _userInputTemplateService.GetTemplate(userIdsRecord, locale);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserInputFormSchema>();
            result.City.Should().Be(_mockedCreateUser.City);
            result.CityHeader.Should().Be("City");
        }
    }
}
