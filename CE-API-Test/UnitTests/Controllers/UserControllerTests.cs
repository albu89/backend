using AutoMapper;
using CE_API_V2.Controllers;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CE_API_Test.TestUtilities;
using CE_API_V2.Services.Interfaces;
using System.Security.Claims;
using CE_API_V2.Models.Records;
using Azure.Communication.Email;
using CE_API_V2.Utility;
using Microsoft.Extensions.Configuration;

namespace CE_API_Test.UnitTests.Controllers
{
    [TestFixture]
    internal class UserControllerTests
    {
        private IMapper _mapper;
        private IUserUOW _userUOW;
        private Mock<IUserUOW> _mockUserUow;
        private UserController _userController;

        private static readonly int _userId = 123;
        private static readonly BiomarkerOrder biomarkerOrder = new();

        #region Setup
        private IInputValidationService _inputValidationService;
        private IUserInformationExtractor _userInformationExtractor;
        private IAdministrativeEntitiesUOW _administrativeEntitiesUow;
        private UserHelper _userHelper;

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
            var result = await _userController.SetPreferences(biomarkerOrder);

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
        public async Task CreatedUser_GivenMockedUserDto_ReturnOkResult()
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
        public async Task RequestAccess_GivenMockedAccessRequestDto_ReturnOkResult()
        {
            //Arrange
            AccessRequest accessRequest = MockDataProvider.GetMockedAccessRequestDto();
            var sut = new UserController(_mapper, _userUOW, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper); 

            //Act
            var result = await sut.RequestAccess(accessRequest);

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
            AccessRequest accessRequest = new AccessRequest();
            var sut = new UserController(_mapper, _userUOW, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);

            //Act
            _ = await sut.RequestAccess(accessRequest);

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
            var sut = new UserController(_mapper, _userUOW, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);
            var expectedReturnedDto = MockDataProvider.GetMockedUser();
           
            //Act
            var currentUser = sut.GetCurrentUser();

            //Assert
            currentUser.Should().NotBeNull();
            currentUser.Should().BeOfType(typeof(OkObjectResult));
            ((OkObjectResult)currentUser).Value.Should().NotBeNull();
            ((OkObjectResult)currentUser).Value.Should().BeOfType(typeof(User));
            ((OkObjectResult)currentUser).Value!.Equals(expectedReturnedDto);
        }
        
        [Test]
        public async Task UpdateUserById_GivenPatchDocumentAndId_ReturnOkResult()
        {
            //Arrange
            var sut = new UsersController(_userUOW, _userInformationExtractor, _mapper, _userHelper);
            var expectedRetrunedDto = MockDataProvider.GetMockedUser();
            var mockedUser = MockDataProvider.GetMockedUser();
            var userId = mockedUser.UserId;

            var patchDocument = MockDataProvider.GetMockedUpdateUserDto();

            //Act
            var currentUser = await sut.UpdateUserById(patchDocument, userId);

            //Assert
            currentUser.Should().NotBeNull();
            currentUser.Should().BeOfType(typeof(OkObjectResult));
            ((OkObjectResult)currentUser).Value.Should().NotBeNull();
            ((OkObjectResult)currentUser).Value.Should().BeOfType(typeof(User));
            ((OkObjectResult)currentUser).Value!.Equals(expectedRetrunedDto);
        }

        [Test]
        public async Task UpdateUser_GivenPatchDocumentAndId_ReturnOkResult()
        {
            //Arrange
            var sut = new UserController(_mapper, _userUOW, _inputValidationService, _userInformationExtractor, _administrativeEntitiesUow, _userHelper);
            var mockedUserDto = MockDataProvider.GetMockedUpdateUserDto();

            //Act
            var currentUser = await sut.UpdateCurrentUser(mockedUserDto);

            //Assert
            currentUser.Should().NotBeNull();
            currentUser.Should().BeOfType(typeof(OkObjectResult));
            ((OkObjectResult)currentUser).Value.Should().NotBeNull();
            ((OkObjectResult)currentUser).Value.Should().BeOfType(typeof(User));
        }
    }
}
