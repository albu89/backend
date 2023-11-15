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
    internal class UsersControllerTests
    {
        #region Setup
        private static readonly BiomarkerOrder biomarkerOrder = new();

        private IConfiguration _configuration;
        private IMapper _mapper;
        private IUserUOW _userUOW;
        private IInputValidationService _inputValidationService;
        private IUserInformationExtractor _userInformationExtractor;
        private IAdministrativeEntitiesUOW _administrativeEntitiesUow;

        private UserController _userController;
        private UserHelper _userHelper;

        #region Mock userdata
        private UserModel _userModel;
        private UserModel _updatedUserModel;
        private string _tenantId = Guid.NewGuid().ToString();
        private string _doctorId = Guid.NewGuid().ToString();
        private string _patientUserId;
        private UserIdsRecord _userIdsRecord;
        #endregion



        [SetUp]
        public void SetUp()
        {
            var administrativeEntitiesUOWMock = new Mock<IAdministrativeEntitiesUOW>();
            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var userUOWMock = new Mock<IUserUOW>();
            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();
            _userIdsRecord = new()
            {
                TenantId = _tenantId,
                UserId = _doctorId,
            };

            var resultUser = MockDataProvider.GetMockedUserModel();
            var userList = MockDataProvider.GetMockedUsersList();

            _patientUserId = Guid.NewGuid().ToString();
            _userModel = new()
            {
                TenantID = _tenantId, //matches doctors tenantId
                UserId = _patientUserId,
                City = "OriginalCity"
            };

            _updatedUserModel = _userModel;
            _updatedUserModel.City = "UpdatedCity";

            var testConfig = new Dictionary<string, string?>()
            {
                { "AzureCommunicationService:Endpoint", "https://ce-v2-communication-services.communication.azure.com/" },
                { "AzureCommunicationService:MailFrom", "DoNotReply@70cd7cba-30aa-4feb-9e09-06c33c97bfb5.azurecomm.net" },
                { "AzureCommunicationService:RequestMailSubject", "CardioExplorer: new user" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfig)
                .Build();

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new MappingProfile());
            });
            
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ValidateAccessRequest(It.IsAny<AccessRequest>())).Returns(true);

            userUOWMock.Setup(u=> u.StoreUser(It.IsAny<UserModel>())).Returns(Task.FromResult(resultUser));
            userUOWMock.Setup(u => u.ProcessAccessRequest(It.IsAny<AccessRequest>())).Returns(Task.FromResult(EmailSendStatus.Succeeded));
            userUOWMock.Setup(u => u.GetUser(It.Is<string>(x=> x == _userModel.UserId), It.IsAny<UserIdsRecord>())).Returns(new UserModel { UserId = $"{_userModel.UserId}", TenantID = $"{_userModel.TenantID}" });  //return value matches patients userID
            userUOWMock.Setup(u => u.GetUser(It.Is<string>(x => x == _userIdsRecord.UserId), It.IsAny<UserIdsRecord>())).Returns(new UserModel { UserId = $"{_userIdsRecord.UserId}", TenantID = $"{_userIdsRecord.TenantId}" }); //return value matches doctors userID
            userUOWMock.Setup(u => u.GetUsersForAdmin(It.IsAny<UserIdsRecord>())).Returns(userList);
            userUOWMock.Setup(u => u.EditBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == "Age")));
            userUOWMock.Setup(u => u.EditBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == ""))).Throws(new Exception("Oh no"));
            userUOWMock.Setup(u => u.StoreBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == "Age")));
            userUOWMock.Setup(u => u.StoreBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == ""))).Throws(new Exception("Oh no"));
            userUOWMock.Setup(u => u.UpdateUser(It.Is<string>(y => y == _userModel.UserId), It.IsAny<UserModel>(), It.Is<UserIdsRecord>(y => y.TenantId == _userModel.TenantID))).Returns(Task.FromResult(_updatedUserModel));
            userUOWMock.Setup(u => u.UpdateUser(It.Is<string>(y => y != _userModel.UserId), It.IsAny<UserModel>(), It.Is<UserIdsRecord>(y => y.TenantId == _userModel.TenantID))).Throws(new Exception("Oh no"));
            userUOWMock.Setup(u => u.UpdateUser(It.Is<string>(y => y == _userModel.UserId), It.IsAny<UserModel>(), It.Is<UserIdsRecord>(y => y.TenantId != _userModel.TenantID))).Throws(new Exception("Oh no"));
            userUOWMock.Setup(u => u.UpdateUser(It.Is<string>(y => y != _userModel.UserId), It.IsAny<UserModel>(), It.Is<UserIdsRecord>(y => y.TenantId != _userModel.TenantID))).Throws(new Exception("Oh no"));

            userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(_userIdsRecord);

            _mapper = config.CreateMapper();
            _administrativeEntitiesUow = administrativeEntitiesUOWMock.Object;
            _userHelper = new UserHelper(_mapper, _configuration);
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
            //Arrange
            
            //Act
            var result = await _userController.ModifyPreferences(biomarkerOrder);
            var currentPreferencesResult = await _userController.GetPreferences();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var resultValue = (result as OkObjectResult).Value;
            currentPreferencesResult.Should().BeOfType<OkObjectResult>();
            var currentPreferences = (currentPreferencesResult as OkObjectResult).Value;

            resultValue.Should().BeEquivalentTo(currentPreferences);
        }

        #endregion

        [Test]
        public async Task UpdateUserById_GivenUpdatedUserWithCorrectIds_ReturnOkResult()  //Correct result -> PatientId must Match - Doctor ID + TenantId Must match 
        {
            //Arrange
            var newCityName = "NewCity";
            CreateUser updatedUser = new()
            {
                //This DTO contains no information regarding the IDs
                City = newCityName,
                FirstName = _userModel.FirstName,
                Surname = _userModel.Surname,
            };

            var sut = new UsersController(_userUOW, _userInformationExtractor, _mapper);

            //Act
            var result = await sut.UpdateUserById(updatedUser, _userModel.UserId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okObjectResult = result as OkObjectResult;

            okObjectResult!.Value.Should().NotBeNull(); 
            okObjectResult.Value.Should().BeOfType(typeof(User));
            var user = (User)okObjectResult.Value!;
            user.FirstName.Should().BeEquivalentTo(_updatedUserModel.FirstName);
            user.Surname.Should().BeEquivalentTo(_updatedUserModel.Surname);
            user.City.Should().BeEquivalentTo(_updatedUserModel.City);
        }

        [Test]
        public async Task UpdateUserById_GivenUpdatedUserWithIncorrectId_ThrowsException()  //Throws exception -> PatientId must Match - Doctor ID + TenantId Must match 
        {
            //Arrange
            var incorrectUserId = Guid.NewGuid().ToString();
            var newCityName = "NewCity";
            CreateUser updatedUser = new();

            var sut = new UsersController(_userUOW, _userInformationExtractor, _mapper);

            //Act
            var updatUserByIdTask = async () => await sut.UpdateUserById(updatedUser, incorrectUserId);

            //Assert
            await updatUserByIdTask.Should().ThrowAsync<Exception>();
        }

        [Test]
        [TestCase("invalidUserId")]
        [TestCase(null)]
        public async Task UpdateUserById_UserInformationExtractorReturnsInvalidTenantId_ThrowsException(string? userId)  //Throws exception -> PatientId must Match - Doctor ID + TenantId Must match 
        {
            //Arrange
            userId ??= _userModel.UserId;
            var userInformation = new UserIdsRecord()
            {
                TenantId = "invalid",
                UserId = userId
            };

            CreateUser updatedUser = new();
            var userInformationExtractor = new Mock<IUserInformationExtractor>();
            userInformationExtractor.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(userInformation);

            var sut = new UsersController(_userUOW, userInformationExtractor.Object, _mapper);

            //Act
            var updatUserByIdTask = async () => await sut.UpdateUserById(updatedUser, userId);

            //Assert
            await updatUserByIdTask.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task GetAllUsers_GivenNoParameter_ReturnOkResult()
        {
            //Arrange
            var sut = new UsersController(_userUOW, _userInformationExtractor, _mapper);

            //Act
            var usersResult = await sut.GetAllUsers();

            //Assert
            usersResult.Should().NotBeNull();
            usersResult.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = usersResult as OkObjectResult;
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType(typeof(List<User>));

            var users = okObjectResult!.Value as List<User>;
            users.Should().NotBeNull();
            users!.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Test]
        public async Task GetUserById_GivenCorrectUserId_ReturnOkResult()
        {
            //Arrange
            var sut = new UsersController(_userUOW, _userInformationExtractor, _mapper);

            //Act
            var userResult = await sut.GetUserById(_userModel.UserId); // Correct patient id is handed over

            //Assert
            userResult.Should().NotBeNull();
            userResult.Should().BeOfType(typeof(OkObjectResult));

            var okObjectResult = userResult as OkObjectResult;
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType(typeof(User));

            var users = okObjectResult!.Value as User;
            users.UserId.Should().Be(_userModel.UserId);
        }

        [Test]
        public async Task GetUserById_GivenUserHasDifferentTenantId_ReturnBadRequestResult()
        {
            //Arrange
            var userIdsRecord = _userIdsRecord;
            userIdsRecord.TenantId = Guid.NewGuid().ToString();
            
            var userInformationExtractor = GetUserInformationExtractor(userIdsRecord);// Returns userIdsRecord with given values
            var sut = new UsersController(_userUOW, userInformationExtractor, _mapper);

            //Act
            var userResult = await sut.GetUserById(_userModel.UserId);

            //Assert
            userResult.Should().NotBeNull();
            userResult.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Test]
        public async Task GetUserById_UserUOWReturnsNull_ReturnBadRequestResult()
        {
            //Arrange
            UserModel? userModel = null;
            var userUowMock = new Mock<IUserUOW>();
            userUowMock.Setup(x => x.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(userModel);

            var sut = new UsersController(userUowMock.Object, _userInformationExtractor, _mapper);

            //Act
            var userResult = await sut.GetUserById(_userModel.UserId);

            //Assert
            userResult.Should().NotBeNull();
            userResult.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        private IUserInformationExtractor GetUserInformationExtractor(UserIdsRecord userIdsRecord)
        {
            var userInformationExtractor = new Mock<IUserInformationExtractor>();
            userInformationExtractor.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>()))
                .Returns(userIdsRecord);

            return userInformationExtractor.Object;
        }
    }
}
