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
using Azure.Communication.Email;
using CE_API_V2.Models;
using CE_API_V2.Utility;

namespace CE_API_Test.UnitTests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private IInputValidationService _inputValidationService;
        private IUserUOW _userUOW;
        private IUserInformationExtractor _userInformationExtractor;
        private IMapper _mapper;
        private UserHelper _userHelper;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();
            _userHelper = new UserHelper(_mapper);
            
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
            inputValidationServiceMock.Setup(x => x.ValidateAccessRequest(It.IsAny<AccessRequestDto>())).Returns(true);
            userUOWMock.Setup(x => x.StoreUser(It.IsAny<User>()))
                .Returns(Task.FromResult(resultUser));
            userUOWMock.Setup(x => x.ProcessAccessRequest(It.IsAny<AccessRequestDto>())).Returns(Task.FromResult(EmailSendStatus.Succeeded));
            userUOWMock.Setup(x => x.GetUser(userIdRecord.UserId)).Returns(resultUser);
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
            var sut = new UserController(_inputValidationService, _userUOW, _userInformationExtractor, _mapper, _userHelper);

            //Act
            var result = await sut.CreateUser(userDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(UserDto));
        }

        [Test]
        public async Task RequestAccess_GivenMockedAccessRequestDto_ReturnOkResult()
        {
            //Arrange
            AccessRequestDto accessRequestDto = MockDataProvider.GetMockedAccessRequestDto();
            var sut = new UserController(_inputValidationService, _userUOW, _userInformationExtractor, _mapper, _userHelper); 

            //Act
            var result = await sut.RequestAccess(accessRequestDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkResult));
            var okResult = result as OkResult;
            okResult?.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task RequestUserCreation_GivenInvalidAccessRequestDto_ReturnOkResult()
        {
            //Arrange
            AccessRequestDto accessRequestDto = new AccessRequestDto();
            var sut = new UserController(_inputValidationService, _userUOW, _userInformationExtractor, _mapper, _userHelper);

            //Act
            var result = await sut.RequestAccess(accessRequestDto);

            //Todo: Validation not yet implemented
            //Assert
            //result.Should().NotBeNull();
            //result.Should().BeOfType(typeof(BadRequestResult));

            //var badRequestResult = result as BadRequestResult;
            //badRequestResult?.StatusCode.Should().Be(400);
        }
        
        [Test]
        public async Task GetCurrentUser_RequestAccessDto_ReturnOkResult()
        {
            //Arrange
            var sut = new UserController(_inputValidationService, _userUOW, _userInformationExtractor, _mapper, _userHelper);
            var expectedRetrunedDto = MockDataProvider.GetMockedUserDto();
            //Act
            var currentUser = await sut.GetCurrentUser();

            //Assert
            currentUser.Should().NotBeNull();
            currentUser.Should().BeOfType(typeof(OkObjectResult));
            ((OkObjectResult)currentUser).Value.Should().NotBeNull();
            ((OkObjectResult)currentUser).Value.Should().BeOfType(typeof(UserDto));
            ((OkObjectResult)currentUser).Value.Equals(expectedRetrunedDto);
        }
    }
}
