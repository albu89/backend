using System.Collections.ObjectModel;
using CE_API_V2.Data;
using CE_API_V2.Models;
using CE_API_V2.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using CE_API_V2.Services.Interfaces;
using Moq;
using System.Security.Claims;
using CE_API_V2.Models.DTO;
using CE_API_Test.TestUtilities;
using CE_API_V2.Models.Records;
using Azure.Communication.Email;
using CE_API_V2.Models.Enum;

namespace CE_API_Test.UnitTests.UnitOfWorks
{
    [TestFixture]
    internal class AdministrativeEntitiesUOWTests
    {
        private DbContextOptions<CEContext> _dbContextOptions;
        private CEContext _context;
        private ICommunicationService _communicationService;

        #region Setup

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CEContext>().UseInMemoryDatabase(databaseName: "CEUnitTestDb").Options;
            _context = new CEContext(_dbContextOptions);

            var inputValidationServiceMock = new Mock<IInputValidationService>();
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);

            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();
            userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(new UserIdsRecord());
            userInfoExtractorMock.Setup(x
                => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(new UserIdsRecord());

            var communicationService = new Mock<ICommunicationService>();
            communicationService.Setup(x
                => x.SendAccessRequest(It.IsAny<AccessRequest>())).Returns(Task.FromResult(EmailSendStatus.Succeeded));

            _communicationService = communicationService.Object;
        }

        #endregion

        #region TearDown

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        #endregion


        [Test]
        public async Task AddCountry_GivenNewCountry_ExpectedStoredCountryInTheDatabase()
        {
            //Arrange
            var sut = new AdministrativeEntitiesUOW(_context);
            var mockedCountry = MockDataProvider.GetMockedCountryModel();

            //Act
            var result = sut.AddCountry(mockedCountry);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(mockedCountry);
            _context.Countries.Should().Contain(mockedCountry);
        }

        [Test]
        public async Task AddOrganization_GivenNewOrganization_ExpectedStoredOrganizationInTheDatabase()
        {
            //Arrange
            var sut = new AdministrativeEntitiesUOW(_context);
            var mockedOrganization = MockDataProvider.GetMockedOrganizationModel();

            //Act
            var result = sut.AddOrganizations(mockedOrganization);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(mockedOrganization);
            _context.Organizations.Should().Contain(mockedOrganization);
        }

        [Test]
        public async Task GetCountryByName_GivenCountryName_ReturnOkResultWithFoundCountry()
        {
            //Arrange
            var country = MockDataProvider.GetMockedCountryModel();
            var countryName = country.Name;

            _context.Countries.Add(country);
            _context.SaveChanges();
            var sut = new AdministrativeEntitiesUOW(_context);

            //Act
            var result = sut.GetCountryByName(countryName);

            //Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(country.Id);
            result!.Name.Should().Be(country.Name);
            result!.ContactEmail.Should().Be(country.ContactEmail);
        }

        [Test]
        public async Task GetCountryByName_GivenNotPresentCountryName_ReturnOkResultWithFoundCountry()
        {
            //Arrange
            var country = MockDataProvider.GetMockedCountryModel();
            var countryName = "NotPresent";

            _context.Countries.Add(country);
            _context.SaveChanges();
            var sut = new AdministrativeEntitiesUOW(_context);

            //Act
            var result = sut.GetCountryByName(countryName);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetOrganizationByName_GivenOrganizationName_ReturnOkResultWithFoundOrganization()
        {
            //Arrange
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var organizationName = organization.Name;

            _context.Organizations.Add(organization);
            _context.SaveChanges();

            var sut = new AdministrativeEntitiesUOW(_context);

            //Act
            var result = sut.GetOrganizationByName(organizationName);

            //Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(organization.Id);
            result!.Name.Should().Be(organization.Name);
            result!.ContactEmail.Should().Be(organization.ContactEmail);
        }

        [Test]
        public async Task GetOrganizationByName_GivenNotPresentOrganizationName_ReturnOkResultWithFoundOrganization()
        {
            //Arrange
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var organizationName = "NotPresent";

            _context.Organizations.Add(organization);
            _context.SaveChanges();
            var sut = new AdministrativeEntitiesUOW(_context);

            //Act
            var result = sut.GetOrganizationByName(organizationName);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task UpdateCountry_GivenUpdatedCountry_ReturnOkResultWithUpdatedCountry()
        {
            //Arrange
            var country = MockDataProvider.GetMockedCountryModel();
            var updatedCountry = new CountryModel()
            {
                Id = country.Id,
                Name = "UpdatedModel",
                ContactEmail = "UpdatedContactEmail"
            };

            using var context = new CEContext(_dbContextOptions);
            context.Countries.Add(country);
            context.SaveChanges();

            using var newContext = new CEContext(_dbContextOptions);
            var sut = new AdministrativeEntitiesUOW(newContext);

            //Act
            var result = sut.UpdateCountry(updatedCountry);
            
            //Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be(updatedCountry.Name);
            result!.ContactEmail.Should().Be(updatedCountry.ContactEmail);
        }

        [Test]
        public async Task UpdateOrganization_GivenUpdatedOrganization_ReturnOkResultWithUpdatedOrganization()
        {
            //Arrange
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var updatedOrganization = new OrganizationModel()
            {
                Id = organization.Id,
                TenantId = organization.TenantId,
                Name = "UpdatedModel",
                ContactEmail = "UpdatedContactEmail"
            };

            using var context = new CEContext(_dbContextOptions);
            context.Organizations.Add(organization);
            context.SaveChanges();

            using var newContext = new CEContext(_dbContextOptions);
            
            var sut = new AdministrativeEntitiesUOW(newContext);

            //Act
            var result = sut.UpdateOrganization(updatedOrganization);

            //Assert
            result.Should().NotBeNull();
            result!.TenantId.Should().Be(organization.TenantId);
            result!.Name.Should().Be(updatedOrganization.Name);
            result!.ContactEmail.Should().Be(updatedOrganization.ContactEmail);
        }

        [Test]
        public async Task DeleteOrganization_GivenOrganizationId_ReturnOkResult()
        {
            //Arrange
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var organization2 = new OrganizationModel()
            {
                ContactEmail = "ContactEmail",
                Name = "Organization2",
                TenantId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
            };

            _context.Organizations.Add(organization);
            _context.Organizations.Add(organization2);
            _context.SaveChanges();
            var sut = new AdministrativeEntitiesUOW(_context);

            var organizationEntities = _context.Organizations.Where(o => o != null);
            organizationEntities.Count().Should().Be(2);
            //Act
            var result = sut.DeleteOrganization(organization.Id);

            //Assert
            result.Should().NotBeNull();
            organizationEntities = _context.Organizations.Where(o => o != null);
            organizationEntities.Count().Should().Be(1);
        }

        [Test]
        public async Task DeleteCountry_GivenUpdatedCountry_ReturnOkResultWithUpdatedCountry()
        {
            //Arrange
            var country = MockDataProvider.GetMockedCountryModel();
            var country2 = new CountryModel()
            {
                ContactEmail = "ContactEmail",
                Name = "country2",
                Id = Guid.NewGuid(),
            };
            _context = new CEContext(_dbContextOptions);
            _context.Countries.Add(country);
            _context.Countries.Add(country2);
            _context.SaveChanges();

            var sut = new AdministrativeEntitiesUOW(_context);

            var countryEntities = _context.Countries.Where(o => o != null);
            countryEntities.Count().Should().Be(2);

            //Act
            var result = sut.DeleteCountry(country.Id);

            //Assert
            result.Should().NotBeNull();
            countryEntities = _context.Countries.Where(o => o != null);
            countryEntities.Count().Should().Be(1);
        }

        [Test]
        public async Task GetCountries_GivenNoParameter_ReturnsListOfCountries()
        {
            //Arrange
            var country = MockDataProvider.GetMockedCountryModel();
            var country2 = new CountryModel()
            {
                ContactEmail = "ContactEmail",
                Name = "country2",
                Id = Guid.NewGuid(),
            };

            _context.Countries.Add(country);
            _context.Countries.Add(country2);
            _context.SaveChanges();

            var sut = new AdministrativeEntitiesUOW(_context);

            //Act
            var result = sut.GetCountries();

            //Assert
            result.Should().BeOfType(typeof(List<CountryModel>));
            var countryList = (List<CountryModel>)result;
            countryList.Count.Should().Be(2);
        }


        [Test]
        public async Task GetOrganizations_GivenNoParameter_ReturnsListOfOrganizations()
        {
            //Arrange
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var organization2 = new OrganizationModel()
            {
                ContactEmail = "ContactEmail",
                Name = "organization2",
                Id = Guid.NewGuid(),
            };

            _context.Organizations.Add(organization);
            _context.Organizations.Add(organization2);
            _context.SaveChanges();
            var sut = new AdministrativeEntitiesUOW(_context);

            //Act
            var result = sut.GetOrganizations();

            //Assert
            result.Should().BeOfType(typeof(List<OrganizationModel>));
            var organizationList = (List<OrganizationModel>)result;
            organizationList.Count.Should().Be(2);
        }

        [Test]
        public void GetUser_GivenId_ExpectedReturnedUser()
        {
            //Arrange
            var mockedUser = MockDataProvider.GetMockedUserModel();
            var mockIds = MockDataProvider.GetUserIdInformationRecord();
            var userId = mockIds.UserId + 5674;
            mockedUser.UserId = userId;
            mockedUser.TenantID = mockIds.TenantId;

            _context.Users.Add(mockedUser);
            _context.SaveChanges();

            var userInfo = new UserIdsRecord() { Role = UserRole.User, UserId = mockedUser.UserId, TenantId = mockedUser.TenantID };

            var sut = new UserUOW(_context, _communicationService);

            //Act
            var returnedUser = sut.GetUser(userId, userInfo);

            //Assert
            returnedUser.Should().NotBeNull();
            returnedUser.Should().BeEquivalentTo(mockedUser);
        }
        
        [Test]
        public async Task UpdateUser_GivenUserAndId_ExpectedReturnedUpdatedUser()
        {
            //Arrange
            var originalUser = MockDataProvider.GetMockedUserModel();

            var newFirstName = "ChangedFirstName";
            var newSurname = "ChangedSurName";

            var newClinicalSetting = PatientDataEnums.ClinicalSetting.SecondaryCare;
            var expectedClinicalSetting = PatientDataEnums.ClinicalSetting.PrimaryCare;

            var newUserRole = UserRole.SystemAdmin;
            var expectedUserRole = UserRole.User;

            var expectedBiomarkerOrder = originalUser.BiomarkerOrders;
            var newBiomarkerOrder = new Collection<BiomarkerOrderModel>
            {
                new() { OrderNumber = 2, BiomarkerId = "first", PreferredUnit = "unit", User = null, UserId = "id" },
                new() { OrderNumber = 1, BiomarkerId = "second", PreferredUnit = "unit", User = null, UserId = "id" }
            };

            originalUser.ClinicalSetting = newClinicalSetting;
            originalUser.Role = newUserRole;
            originalUser.BiomarkerOrders = expectedBiomarkerOrder;
            originalUser.TenantID = "tenant1";

            _context.Users.Add(originalUser);
            _context.SaveChanges();

            var updatedUser = MockDataProvider.GetMockedUserModel();
            updatedUser.FirstName = newFirstName;
            updatedUser.Surname = newSurname;
            
            updatedUser.BiomarkerOrders = newBiomarkerOrder;
            var sut = new UserUOW(_context, _communicationService);

            var adminInfo = new UserIdsRecord() { UserId = "Admin1", TenantId = originalUser.TenantID, Role = UserRole.Admin };

            //Act
            var returnedUser = await sut.UpdateUser(originalUser.UserId, updatedUser, adminInfo);

            //Assert
            returnedUser.Should().NotBeNull();
            returnedUser.FirstName.Should().Be(newFirstName);
            returnedUser.Surname.Should().Be(newSurname);
            returnedUser.BiomarkerOrders.Should().NotBeNullOrEmpty();
            returnedUser.BiomarkerOrders.FirstOrDefault(x => x.BiomarkerId == "first").Should().NotBeNull();
            returnedUser.BiomarkerOrders.FirstOrDefault(x => x.BiomarkerId == "first")!.OrderNumber.Should().Be(1);
            returnedUser.ClinicalSetting.Should().NotBe(expectedClinicalSetting);
            returnedUser.Role.Should().NotBe(expectedUserRole);
        }
    }
}
