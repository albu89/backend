using AutoMapper;
using Azure.Communication.Email;
using CE_API_Test.TestUtilities;
using CE_API_V2.Controllers;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;

namespace CE_API_Test.UnitTests.Controllers
{
    [TestFixture]
    internal class UserControllerTests
    {
        private static readonly int _userId = 123;
        private static readonly BiomarkerOrder biomarkerOrder = new();

        private IMapper _mapper;
        private IUserUOW _userUOW;
        private Mock<IUserUOW> _mockUserUow;
        private UserController _userController;
        private IInputValidationService _inputValidationService;
        private IUserInformationExtractor _userInformationExtractor;
        private IAdministrativeEntitiesUOW _administrativeEntitiesUow;
        private UserHelper _userHelper;

        #region Setup

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();

            _mockUserUow = new Mock<IUserUOW>();
            _mockUserUow.Setup(u => u.EditBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == "Age")));
            _mockUserUow.Setup(u => u.EditBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == ""))).Throws(new Exception("Oh no"));
            _mockUserUow.Setup(u => u.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(new UserModel() { UserId = $"{_userId}" });
            _userUOW = _mockUserUow.Object;

            var inMemSettings = new Dictionary<string, string>
            {
                { "AiSubpath", Resources.TestResources.AiSubpath }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemSettings!).Build();

            _userHelper = new UserHelper(_mapper, configuration);
            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var userUOWMock = new Mock<IUserUOW>();
            var administrativeEntitiesUOWMock = new Mock<IAdministrativeEntitiesUOW>();
            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();
            var userIdRecord = new UserIdsRecord()
            {
                TenantId = "MockedTenantId",
                UserId = "MockedUserId",
            };

            var resultUser = MockDataProvider.GetMockedUserModel();
            resultUser.UserId = _userId.ToString();
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ValidateAccessRequest(It.IsAny<AccessRequest>())).Returns(true);
            userUOWMock.Setup(x => x.StoreUser(It.IsAny<UserModel>()))
                .Returns(Task.FromResult(resultUser));
            userUOWMock.Setup(x => x.ProcessAccessRequest(It.IsAny<AccessRequest>())).Returns(Task.FromResult(EmailSendStatus.Succeeded));
            userUOWMock.Setup(x => x.GetUser(userIdRecord.UserId, It.IsAny<UserIdsRecord>())).Returns(resultUser);
            userUOWMock.Setup(x => x.UpdateUser(It.IsAny<string>(), It.IsAny<UserModel>(), It.IsAny<UserIdsRecord>()))
                .Returns(Task.FromResult(new UserModel()));
            userUOWMock.Setup(u => u.EditBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == "Age")));
            userUOWMock.Setup(u => u.EditBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == ""))).Throws(new Exception("Oh no"));
            userUOWMock.Setup(u => u.CheckIfIsActiveStateIsModifiable(It.IsAny<UserIdsRecord>())).Returns(true);

            userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(userIdRecord);
            
            _administrativeEntitiesUow = administrativeEntitiesUOWMock.Object;
            _inputValidationService = inputValidationServiceMock.Object;
            _userUOW = userUOWMock.Object;
            _userInformationExtractor = userInfoExtractorMock.Object;
            _userController = new UserController(_mapper, _userUOW, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);
        }

        #endregion

        #region PostBiomarkerOrder

        [Test]
        public async Task PostBiomarkerOrder_ReturnsOk()
        {
            //Arrange

            //Act
            var result = await _userController.SetPreferences(biomarkerOrder);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region EditBiomarkerOrder

        [Test]
        public async Task EditBiomarkerOrder_ReturnsOk()
        {
            var result = await _userController.ModifyPreferences(biomarkerOrder);

            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        [Test]
        public async Task CreateUser_GivenMockedUserDto_ReturnOkResult()
        {
            //Arrange
            CreateUser user = MockDataProvider.GetMockedCreateUser();
            var sut = new UserController(_mapper, _userUOW, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);

            //Act
            var result = await sut.CreateUser(user);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(User));
        }

        [Test]
        public async Task CreateUser_InputValidationServiceReturnsInvalidUserResult_ReturnBadRequestResult()
        {
            //Arrange
            var inputValidationService = new Mock<IInputValidationService>();
            inputValidationService.Setup(X => X.ValidateUser(It.IsAny<CreateUser>())).Returns(false);
            CreateUser user = MockDataProvider.GetMockedCreateUser();

            var sut = new UserController(_mapper, _userUOW, inputValidationService.Object, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);

            //Act
            var result = await sut.CreateUser(user);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Test]
        public async Task CreateUser_UserInformationExtractorReturnsInvalidUserInformation_ReturnBadRequestResult()
        {
            //Arrange
            var inputValidationService = GetInputValidationServiceMockWithFalseResult();
            CreateUser user = MockDataProvider.GetMockedCreateUser();

            var sut = new UserController(_mapper, _userUOW, inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);

            //Act
            var result = await sut.CreateUser(user);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Test]
        public async Task RequestAccess_InputValidationServiceReturnsFalse_ReturnBadRequestResult()
        {
            //Arrange
            AccessRequest accessRequest = MockDataProvider.GetMockedAccessRequestDto();
            var inputValidationService = GetInputValidationServiceMockWithFalseResult();

            var sut = new UserController(_mapper, _userUOW, inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);

            //Act
            var result = await sut.RequestAccess(accessRequest);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Test]
        public async Task RequestAccess_UserUowReturnsReturnsEmailSendStatusFailed_ReturnBadRequestResult()
        {
            //Arrange
            AccessRequest accessRequest = MockDataProvider.GetMockedAccessRequestDto();
            var userUowMock = GetUserUowWithFalseResults();

            var sut = new UserController(_mapper, userUowMock, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);

            //Act
            var result = await sut.RequestAccess(accessRequest);
            
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Test]
        public async Task RequestUserCreation_InputValidationServiceReturnsFalse_ReturnOkResult()
        {
            //Arrange
            var inputValidationService = GetInputValidationServiceMockWithFalseResult();

            AccessRequest accessRequest = new AccessRequest();
            var sut = new UserController(_mapper, _userUOW, inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);

            //Act
            var result = await sut.RequestAccess(accessRequest);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Test]
        public async Task RequestUserCreation_UserUowReturnsReturnsEmailSendStatusFailed_ReturnBadRequestResult()
        {
            //Arrange
            AccessRequest accessRequest = new AccessRequest();
            var userUowMock = GetUserUowWithFalseResults();

            var sut = new UserController(_mapper, userUowMock, _inputValidationService, _userInformationExtractor,
                _administrativeEntitiesUow, _userHelper);

            //Act
            var result = await sut.RequestAccess(accessRequest);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Test]
        public async Task GetCurrentUser_GivenRequestAccessDto_ReturnOkResult()
        {
            //Arrange
            var sut = new UserController(_mapper, _userUOW, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);
            var expectedReturnedDto = MockDataProvider.GetMockedCreateUser();
       
            //Act
            var result = sut.GetCurrentUser();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = result as OkObjectResult;
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(User));
            okObjectResult!.Value!.Equals(expectedReturnedDto);
        }

        [Test]
        public async Task GetCurrentUser_UserUowReturnsNull_ReturnOkResult()
        {
            //Arrange
            var userUow = GetUserUowWithFalseResults();

            var sut = new UserController(_mapper, userUow, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);

            
            //Act
            var result = sut.GetCurrentUser();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Test]
        public async Task UpdateUserById_GivenPatchDocumentAndId_ReturnOkResult()
        {
            //Arrange
            var sut = new UsersController(_userUOW, _userInformationExtractor, _mapper);
            var expectedReturnedDto = MockDataProvider.GetMockedCreateUser();
            var mockedUser = MockDataProvider.GetMockedUser();
            var userId = mockedUser.UserId;

            var patchDocument = MockDataProvider.GetMockedUpdateUserDto();

            //Act
            var result = await sut.UpdateUserById(patchDocument, userId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = result as OkObjectResult;
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(User));
            okObjectResult!.Value!.Equals(expectedReturnedDto);
        }

        [Test]
        public async Task UpdateUser_GivenUserDto_ReturnOkResult()
        {
            //Arrange
            var sut = new UserController(_mapper, _userUOW, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);
            var mockedUserDto = MockDataProvider.GetMockedUpdateUserDto();

            //Act
            var result = await sut.UpdateCurrentUser(mockedUserDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = result as OkObjectResult;
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(User));
        }

        private IInputValidationService GetInputValidationServiceMockWithFalseResult()
        {
            var inputValidationService = new Mock<IInputValidationService>();
            inputValidationService.Setup(X => X.ValidateUser(It.IsAny<CreateUser>())).Returns(false);
            inputValidationService.Setup(X => X.ValidateAccessRequest(It.IsAny<AccessRequest>())).Returns(false);
            inputValidationService.Setup(X => X.ValidateOrganization(It.IsAny<CreateOrganization>())).Returns(false);

            return inputValidationService.Object;
        }

        private IUserUOW GetUserUowWithFalseResults()
        {
            UserModel? userModel = null;
            var userUowMock = new Mock<IUserUOW>();
            userUowMock.Setup(u => u.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(new UserModel() { UserId = $"{_userId}" });
            userUowMock.Setup(u => u.ProcessAccessRequest(It.IsAny<AccessRequest>())).Returns(Task.FromResult(EmailSendStatus.Failed));
            userUowMock.Setup(u => u.GetUser(It.IsAny<string>(),It.IsAny<UserIdsRecord>())).Returns(userModel);

            return userUowMock.Object;
        }
    }
}
