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
    internal class UsersControllerTests
    {
        private IConfiguration _configuration;
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
            _mapper = config.CreateMapper();

            var administrativeEntitiesUOWMock = new Mock<IAdministrativeEntitiesUOW>();

            _administrativeEntitiesUow = administrativeEntitiesUOWMock.Object;
            _mockUserUow = new Mock<IUserUOW>();
            _mockUserUow.Setup(u => u.EditBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == "Age")));
            _mockUserUow.Setup(u => u.EditBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == ""))).Throws(new Exception("Oh no"));
            _mockUserUow.Setup(u => u.StoreBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == "Age")));
            _mockUserUow.Setup(u => u.StoreBiomarkerOrderEntry(It.Is<BiomarkerOrderModel>(x => x.BiomarkerId == ""))).Throws(new Exception("Oh no"));
            _mockUserUow.Setup(u => u.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(new UserModel() { UserId = $"{_userId}" });
            _userUOW = _mockUserUow.Object;


            _userHelper = new UserHelper(_mapper, _configuration);
            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var userUOWMock = new Mock<IUserUOW>();
            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();
            var userIdRecord = new UserIdsRecord()
            {
                TenantId = "MockedTentantId",
                UserId = "MockedUserId",
            };

            var resultUser = MockDataProvider.GetMockedUser();
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ValidateAccessRequest(It.IsAny<AccessRequest>())).Returns(true);
            userUOWMock.Setup(x => x.StoreUser(It.IsAny<UserModel>()))
                .Returns(Task.FromResult(resultUser));
            userUOWMock.Setup(x => x.ProcessAccessRequest(It.IsAny<AccessRequest>())).Returns(Task.FromResult(EmailSendStatus.Succeeded));
            userUOWMock.Setup(x => x.GetUser(userIdRecord.UserId, It.IsAny<UserIdsRecord>())).Returns(resultUser);
            userUOWMock.Setup(x => x.UpdateUser(It.IsAny<string>(), It.IsAny<UserModel>(),It.IsAny<UserIdsRecord>()))
                .Returns(Task.FromResult(new UserModel()));
            userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(userIdRecord);

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
        public async Task UpdateUserById_GivenUpdatedUserAndId_ReturnOkResult()
        {
            //Arrange
            var sut = new UsersController(_inputValidationService, _userUOW, _userInformationExtractor, _mapper, _userHelper);
            var userId = MockDataProvider.GetMockedUser().UserId;
            var updatedUser = MockDataProvider.GetMockedCreateUserDto();

            //Act
            var currentUser = await sut.UpdateUserById(updatedUser, userId);

            //Assert
            currentUser.Should().NotBeNull();
            currentUser.Should().BeOfType(typeof(OkObjectResult));
            ((OkObjectResult)currentUser).Value.Should().NotBeNull();
            ((OkObjectResult)currentUser).Value.Should().BeOfType(typeof(User));
            ((OkObjectResult)currentUser).Value!.Equals(updatedUser);
        }
    }
}
