using CE_API_Test.TestUtilities;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Moq;
using System.Security.Claims;
using AutoMapper;
using CE_API_V2.Controllers;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Models.Records;
using Microsoft.AspNetCore.Mvc;

namespace CE_API_Test.UnitTests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private IInputValidationService _inputValidationService;
        private IUserUOW _userUOW;
        private IUserInformationExtractor _userInformationExtractor;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();
            
            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var userUOWMock = new Mock<IUserUOW>();
            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();
            var userIdRecord = new UserIdsRecord()
            {
                TenantId = "MockedTentantId",
                UserId = "MockedUserId",
            };

            var resultUser = MockDataProvider.GetMockedUser();
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUserDto>())).Returns(true);
            userUOWMock.Setup(x => x.StoreUser(It.IsAny<CreateUserDto>(), It.IsAny<UserIdsRecord>()))
                .Returns(Task.FromResult(resultUser));
            userInfoExtractorMock.Setup(x=> x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(userIdRecord);
            
            _inputValidationService = inputValidationServiceMock.Object;
            _userUOW = userUOWMock.Object;
            _userInformationExtractor = userInfoExtractorMock.Object;
        }

        [Test]
        public async Task CreatedUser_GivenMockedUserDto_ReturnOkResult()
        {
            //Arrange
            CreateUserDto userDto = MockDataProvider.GetMockedCreateUserDto();
            var sut = new UserController(_inputValidationService, _userUOW, _userInformationExtractor, _mapper);

            //Act
            var result = await sut.CreateUser(userDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(UserDto));
        }
    }
}
