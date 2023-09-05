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

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();

            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();
            var userIdRecord = new UserIdsRecord()
            {
                TenantId = "MockedTenantId",
                UserId = "MockedUserId",
            };

            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ValidateAccessRequest(It.IsAny<AccessRequest>())).Returns(true);
            inputValidationServiceMock.Setup(x => x.ValidateOrganization(It.IsAny<CreateOrganization>())).Returns(true);
            userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(userIdRecord);
            
            _administrativeEntitiesUow = GetAdministrativeEntitiesUowMock();
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
                TenantId = Guid.NewGuid(),
            };

            var mockedOrganizationModel2 = new OrganizationModel()
            {
                Name = "Organization2",
                ContactEmail = "ContactEmail2",
                Id = Guid.NewGuid(),
                TenantId = Guid.NewGuid(),
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
            administrativeEntitiesUOWMock.Setup(x => x.AddOrganizations(It.IsAny<OrganizationModel>()))
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

        [Test]
        public async Task GetCountries_ReturnListOfCountries()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUow, _userInformationExtractor);

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
        public async Task GetOrganizations_ReturnListOfOrganizations()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUow, _userInformationExtractor);

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
        public async Task AddCountry_GivenCountry_ReturnsOkResultWithStoredCountry()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUow, _userInformationExtractor);

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
        public async Task AddOrganization_GivenOrganization_ReturnsOkResultWithStoredOrganization()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUow, _userInformationExtractor);

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
        public async Task UpdateCountry_GivenUpdatedCountry_ReturnsOkResultWithUpdatedCountry()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUow, _userInformationExtractor);

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
        public async Task UpdateOrganization_GivenUpdatedOrganization_ReturnsOkResultWithUpdatedOrganization()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUow, _userInformationExtractor);

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
        public async Task DeleteOrganization_GivenIdOfOrganization_ReturnsOkResult()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUow, _userInformationExtractor);

            //Act
            var result = sut.DeleteOrganizationById(new Guid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkResult));
            var okResult = result as OkResult;
            okResult?.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task DeleteCountry_GivenIdOfCountry_ReturnsOkResult()
        {
            //Arrange
            var sut = new AdministrativeEntitiesController(_mapper, _inputValidationService, _administrativeEntitiesUow, _userInformationExtractor);

            //Act
            var result = sut.DeleteCountryById(new Guid());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkResult));
            var okResult = result as OkResult;
            okResult?.StatusCode.Should().Be(200);
        }
    }
}
