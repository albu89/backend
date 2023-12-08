using AutoMapper;
using CE_API_V2.Controllers;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using CE_API_Test.TestUtilities;
using CE_API_V2.Data;
using Microsoft.EntityFrameworkCore;
using CE_API_V2.Utility;
using Microsoft.Extensions.Configuration;

namespace CE_API_Test.UnitTests.Controllers
{
    [TestFixture]
    internal class AdministrativeEntitiesControllerTests
    {
        #region Setup

        private IMapper _mapper;
        private IInputValidationService _inputValidationService;
        private IUserInformationExtractor _userInformationExtractor;
        private IAdministrativeEntitiesUOW _administrativeEntitiesUow;
        private IAdministrativeEntitiesUOW _administrativeEntitiesUowWithNullReturnValues;
        private ICommunicationService _communicationService;
        private IUserUOW _userUOW;
        private DbContextOptions<CEContext> _dbContextOptions;
        private CEContext _context;
        private UserHelper _userHelper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();

            var inMemSettings = new Dictionary<string, string>
            {
                { "AiSubpath", Resources.TestResources.AiSubpath }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemSettings!).Build();

            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();
            var userUOW = new Mock<IUserUOW>();
            var userIdRecord = new UserIdsRecord()
            {
                TenantId = "MockedTenantId",
                UserId = "MockedUserId",
            };

            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ValidateAccessRequest(It.IsAny<AccessRequest>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ValidateOrganization(It.IsAny<CreateOrganization>())).Returns(true);
            userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(userIdRecord);
            userUOW.Setup(u => u.GetUser(It.IsAny<string>(), It.IsAny<UserIdsRecord>())).Returns(MockDataProvider.GetMockedUserModel);

            _userUOW = userUOW.Object;
            _userHelper = new UserHelper(_mapper, configuration);
            _administrativeEntitiesUow = GetAdministrativeEntitiesUowMock();
            _administrativeEntitiesUowWithNullReturnValues = GetAdministrativeEntitiesUowMockWithNullReturnValues();
            _inputValidationService = inputValidationServiceMock.Object;
            _userInformationExtractor = userInfoExtractorMock.Object;
        }
        #endregion

        private IAdministrativeEntitiesUOW GetAdministrativeEntitiesUowMock()
        {
            var mockedCountryModel1 = new CountryModel()
            {
                Name = "Country1",
                ContactEmail = "ContactEmail1",
                Id = Guid.NewGuid()
            };

            var mockedCountryModel2 = new CountryModel()
            {
                Name = "Country2",
                ContactEmail = "ContactEmail2",
                Id = Guid.NewGuid()
            };

            var mockedOrganizationModel1 = new OrganizationModel()
            {
                Name = "Organization1",
                ContactEmail = "ContactEmail1",
                Id = Guid.NewGuid(),
                TenantId = Guid.NewGuid().ToString(),
            };

            var mockedOrganizationModel2 = new OrganizationModel()
            {
                Name = "Organization2",
                ContactEmail = "ContactEmail2",
                Id = Guid.NewGuid(),
                TenantId = Guid.NewGuid().ToString(),
            };

            var administrativeEntitiesUOWMock = new Mock<IAdministrativeEntitiesUOW>();
            administrativeEntitiesUOWMock.Setup(x => x.GetCountries()).Returns(new List<CountryModel>() 
                { mockedCountryModel1, mockedCountryModel2 });
            administrativeEntitiesUOWMock.Setup(x => x.GetOrganizations()).Returns(new List<OrganizationModel>()
                { mockedOrganizationModel1, mockedOrganizationModel2 });
         
            administrativeEntitiesUOWMock.Setup(x => x.GetOrganizationByName(It.IsAny<string>()))
                .Returns(mockedOrganizationModel1);
            administrativeEntitiesUOWMock.Setup(x => x.GetCountryByName(It.IsAny<string>()))
                .Returns(mockedCountryModel1);
            administrativeEntitiesUOWMock.Setup(x => x.AddCountry(It.IsAny<CountryModel>()))
                .Returns(mockedCountryModel1);
            administrativeEntitiesUOWMock.Setup(x => x.AddOrganization(It.IsAny<OrganizationModel>()))
                .Returns(mockedOrganizationModel1);
            administrativeEntitiesUOWMock.Setup(x => x.UpdateCountry(It.IsAny<CountryModel>()))
                .Returns(mockedCountryModel1);
            administrativeEntitiesUOWMock.Setup(x => x.UpdateOrganization(It.IsAny<OrganizationModel>()))
                .Returns(mockedOrganizationModel1);
            administrativeEntitiesUOWMock.Setup(x => x.DeleteOrganization(It.IsAny<Guid>()))
                .Returns(mockedOrganizationModel1);
            administrativeEntitiesUOWMock.Setup(x => x.DeleteCountry(It.IsAny<Guid>()))
                .Returns(mockedCountryModel1);
            return administrativeEntitiesUOWMock.Object;
        }

        private IAdministrativeEntitiesUOW GetAdministrativeEntitiesUowMockWithNullReturnValues()
        {
            OrganizationModel? organizationModel = null;
            CountryModel? countryModel = null;

            var administrativeEntitiesUowMock = new Mock<IAdministrativeEntitiesUOW>();
            administrativeEntitiesUowMock.Setup(x => x.GetCountries()).Returns(new List<CountryModel>());
            administrativeEntitiesUowMock.Setup(x => x.GetOrganizations()).Returns(new List<OrganizationModel>());
            administrativeEntitiesUowMock.Setup(x => x.GetOrganizationByName(It.IsAny<string>())).Returns(organizationModel);
            administrativeEntitiesUowMock.Setup(x => x.GetCountryByName(It.IsAny<string>())).Returns(countryModel);
            administrativeEntitiesUowMock.Setup(x => x.AddCountry(It.IsAny<CountryModel>())).Returns(countryModel);
            administrativeEntitiesUowMock.Setup(x => x.AddOrganization(It.IsAny<OrganizationModel>())).Returns(organizationModel);
            administrativeEntitiesUowMock.Setup(x => x.UpdateCountry(It.IsAny<CountryModel>())).Returns(countryModel);
            administrativeEntitiesUowMock.Setup(x => x.UpdateOrganization(It.IsAny<OrganizationModel>())).Returns(organizationModel);
            administrativeEntitiesUowMock.Setup(x => x.DeleteOrganization(It.IsAny<Guid>())).Returns(organizationModel);
            administrativeEntitiesUowMock.Setup(x => x.DeleteCountry(It.IsAny<Guid>())).Returns(countryModel);

            return administrativeEntitiesUowMock.Object;
        }

        [Test]
        public void GetCountries_ReturnListOfCountries()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper,
                                                           _inputValidationService,
                                                           _administrativeEntitiesUow,
                                                           _userInformationExtractor,
                                                           _userUOW, _userHelper);

            //Act
            var result = sut.GetCountries();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(List<Country>));
            ((List<Country>)okResult?.Value).Count.Should().Be(2);
        }

        [Test]
        public void GetCountries_GivenEmptyCountryList_ReturnNotFoundResult()
        {
            //Arrange
            var administrativeEntitiesUOW = new Mock<IAdministrativeEntitiesUOW>();
            administrativeEntitiesUOW.Setup(x => x.GetCountries()).Returns(new List<CountryModel>());

            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, administrativeEntitiesUOW.Object, _userInformationExtractor, _userUOW, _userHelper);

            //Act
            var result = sut.GetCountries();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Test]
        public void GetOrganizations_ReturnListOfOrganizations()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, 
                                                           _inputValidationService, 
                                                           _administrativeEntitiesUow, 
                                                           _userInformationExtractor, 
                                                           _userUOW, _userHelper);

            //Act
            var result = sut.GetOrganizations();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(List<Organization>));
            ((List<Organization>)okResult?.Value).Count.Should().Be(2);
        }

        [Test]
        public void GetOrganizations_GivenEmptyCountryList_ReturnNotFoundResult()
        {
            //Arrange
            var administrativeEntitiesUOW = new Mock<IAdministrativeEntitiesUOW>();
            administrativeEntitiesUOW.Setup(x => x.GetOrganizations()).Returns(new List<OrganizationModel>());

            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, administrativeEntitiesUOW.Object, _userInformationExtractor, _userUOW, _userHelper);

            //Act
            var result = sut.GetOrganizations();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Test]
        public void AddCountry_GivenCountry_ReturnsOkResultWithStoredCountry()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper,
                                                           _inputValidationService,
                                                           _administrativeEntitiesUow,
                                                           _userInformationExtractor,
                                                           _userUOW, 
                                                           _userHelper);

            //Act
            var result = sut.AddCountry(new CreateCountry());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(CreateCountry));
        }

        [Test]
        public void AddCountry_UowReturnsNull_ReturnsBadRequestResult()
        {
            //Arrange
            var administrativeEntitiesUow = new Mock<IAdministrativeEntitiesUOW>();
            CountryModel? returnedCountryModel = null;
            administrativeEntitiesUow.Setup(x => x.AddCountry(It.IsAny<CountryModel>())).Returns(returnedCountryModel);

            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, administrativeEntitiesUow.Object, _userInformationExtractor, _userUOW, _userHelper);

            //Act
            var result = sut.AddCountry(new CreateCountry());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Test]
        public void AddOrganization_GivenOrganization_ReturnsOkResultWithStoredOrganization()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper,
                                                           _inputValidationService,
                                                           _administrativeEntitiesUow,
                                                           _userInformationExtractor,
                                                           _userUOW, _userHelper);
            //Act
            var result = sut.AddOrganization(new CreateOrganization());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(Organization));
        }

        [Test]
        public void AddOrganization_UowReturnsNull_ReturnsBadRequestResult()
        {
            //Arrange
            var administrativeEntitiesUOW = new Mock<IAdministrativeEntitiesUOW>();
            OrganizationModel? returnedOrganizationModel = null;
            administrativeEntitiesUOW.Setup(x => x.AddOrganization(It.IsAny<OrganizationModel>())).Returns(returnedOrganizationModel);
            
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, administrativeEntitiesUOW.Object, _userInformationExtractor, _userUOW, _userHelper);

            //Act
            var result = sut.AddOrganization(new CreateOrganization());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Test]
        public void UpdateCountry_GivenUpdatedCountry_ReturnsOkResultWithUpdatedCountry()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper,
                                                           _inputValidationService,
                                                           _administrativeEntitiesUow,
                                                           _userInformationExtractor,
                                                           _userUOW, _userHelper);

            //Act
            var result = sut.UpdateCountry(Guid.NewGuid(), new CreateCountry());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(CreateCountry));
        }

        [Test]
        public void UpdateCountry_UowReturnsNull_ReturnsBadRequest()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUowWithNullReturnValues, _userInformationExtractor, _userUOW, _userHelper);

            //Act
            var result = sut.UpdateCountry(Guid.NewGuid(), new CreateCountry());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Test]
        public void UpdateOrganization_GivenUpdatedOrganization_ReturnsOkResultWithUpdatedOrganization()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper,
                                                           _inputValidationService,
                                                           _administrativeEntitiesUow,
                                                           _userInformationExtractor,
                                                           _userUOW, _userHelper);

            //Act
            var result = sut.UpdateOrganization(Guid.NewGuid(), new CreateOrganization());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeOfType(typeof(Organization));
        }

        [Test]
        public void UpdateOrganization_ValidationServiceReturnsFalseParameter_ReturnsBadRequest()
        {
            //Arrange
            var inputValidationService = new Mock<IInputValidationService>();
            inputValidationService.Setup(x=> x.ValidateOrganization(It.IsAny<CreateOrganization>())).Returns(false);
            var sut = new AdministrativeEntitiesController(_mapper, inputValidationService.Object, _administrativeEntitiesUow, _userInformationExtractor, _userUOW, _userHelper);

            //Act
            var result = sut.UpdateOrganization(Guid.NewGuid(), new CreateOrganization());

            //Assert
           result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void UpdateOrganization_UowReturnsNull_ReturnsBadRequest()
        {
            //Arrange   
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUowWithNullReturnValues, _userInformationExtractor, _userUOW, _userHelper);

            //Act
            var result = sut.UpdateOrganization(Guid.NewGuid(), new CreateOrganization());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Test]
        public void DeleteOrganization_GivenIdOfOrganization_ReturnsOkResult()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper,
                                                           _inputValidationService,
                                                           _administrativeEntitiesUow,
                                                           _userInformationExtractor,
                                                           _userUOW, _userHelper);
            //Act
            var result = sut.DeleteOrganizationById(new Guid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkResult));
            var okResult = result as OkResult;
            okResult?.StatusCode.Should().Be(200);
        }

        [Test]
        public void DeleteOrganization_UowReturnsNull_ReturnsBadRequestResult()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUowWithNullReturnValues, _userInformationExtractor, _userUOW, _userHelper);

            //Act
            var result = sut.DeleteOrganizationById(new Guid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Test]
        public void DeleteCountry_GivenIdOfCountry_ReturnsOkResult()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper,
                                                           _inputValidationService,
                                                           _administrativeEntitiesUow,
                                                           _userInformationExtractor,
                                                           _userUOW, _userHelper);
            //Act
            var result = sut.DeleteCountryById(new Guid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkResult));
            var okResult = result as OkResult;
            okResult?.StatusCode.Should().Be(200);
        }

        [Test]
        public void DeleteCountry_UowReturnsNull_ReturnsBadRequestResult()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUowWithNullReturnValues, _userInformationExtractor, _userUOW, _userHelper);

            //Act
            var result = sut.DeleteCountryById(new Guid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
