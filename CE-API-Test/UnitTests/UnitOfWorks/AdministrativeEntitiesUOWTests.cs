using Azure.Communication.Email;
using CE_API_Test.TestUtilities;
using CE_API_V2.Data;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Enum;
using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.ObjectModel;
using CE_API_V2.UnitOfWorks.Interfaces;

namespace CE_API_Test.UnitTests.UnitOfWorks
{
    [TestFixture]
    internal class AdministrativeEntitiesUOWTests
    {
        private ICommunicationService _communicationService;
        private IOrganisationUOW _organisationUow;
        private DbContextOptions<CEContext> _dbContextOptions;
        private CEContext _context;
        #region Setup

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CEContext>().UseInMemoryDatabase(databaseName: "CEUnitTestDb").Options;
            _context = new CEContext(_dbContextOptions);

            var organizationUow = new Mock<IOrganisationUOW>();
            var communicationService = new Mock<ICommunicationService>();
            communicationService.Setup(x => x.SendAccessRequest(It.IsAny<AccessRequest>())).Returns(Task.FromResult(EmailSendStatus.Succeeded));

            _communicationService = communicationService.Object;
            _organisationUow = organizationUow.Object;
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
        public void AddCountry_GivenNewCountry_ExpectedStoredCountryInTheDatabase()
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
        public void AddCountry_GivenFaultyCountry_ExpectedStoredCountryInTheDatabase()
        {
            //Arrange
            var sut = new AdministrativeEntitiesUOW(_context);
            var mockedCountry = MockDataProvider.GetMockedCountryModel();
            mockedCountry.Name = null;

            //Act
            var addCountryTask = () => sut.AddCountry(mockedCountry);

            //Assert
             addCountryTask.Should().Throw<Exception>();
        }

        [Test]
        public void AddCountry_GivenDuplicatedCountry_ExpectedStoredCountryInTheDatabase()
        {
            //Arrange
            var sut = new AdministrativeEntitiesUOW(_context);
            var mockedCountry = MockDataProvider.GetMockedCountryModel();
            mockedCountry.Name = null;

            //Act
            var addCountryTask = () => sut.AddCountry(mockedCountry);

            //Assert
            addCountryTask.Should().Throw<Exception>();
        }

        [Test]
        public void AddOrganization_GivenNewOrganization_ExpectedStoredOrganizationInTheDatabase()
        {
            //Arrange
            var sut = new AdministrativeEntitiesUOW(_context);
            var mockedOrganization = MockDataProvider.GetMockedOrganizationModel();

            //Act
            var result = sut.AddOrganization(mockedOrganization);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(mockedOrganization);
            _context.Organizations.Should().Contain(mockedOrganization);
            _context.Organizations.FirstOrDefault(x => x.Id == mockedOrganization.Id).Userquota.Should().Be(mockedOrganization.Userquota);
            _context.Organizations.FirstOrDefault(x => x.Id == mockedOrganization.Id).ContactEmail.Should().Be(mockedOrganization.ContactEmail);
            _context.Organizations.FirstOrDefault(x => x.Id == mockedOrganization.Id).Name.Should().Be(mockedOrganization.Name);
            _context.Organizations.FirstOrDefault(x => x.Id == mockedOrganization.Id).TenantId.Should().Be(mockedOrganization.TenantId);
        }

        [Test]
        public void AddOrganization_GivenDuplicatedOrganizationId_ExpectedNotStoredOrganizationInTheDatabase()
        {
            //Arrange
            var sut = new AdministrativeEntitiesUOW(_context);
            var mockedOrganization = MockDataProvider.GetMockedOrganizationModel();
            var duplicatedMockedOrganization = mockedOrganization;

            //Act
            var addOrganization1Task = () => sut.AddOrganization(mockedOrganization);
            var addOrganization2Task = () => sut.AddOrganization(duplicatedMockedOrganization);

            //Assert
            addOrganization1Task.Should().NotThrow<Exception>();
            addOrganization2Task.Should().Throw<Exception>();
        }

        [Test]
        public void GetCountryByName_GivenCountryName_ReturnFoundCountry()
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
        public void GetCountryByName_GivenNotPresentCountryName_ReturnNull()
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
        public void GetOrganizationByName_GivenOrganizationName_ReturnOrganization()
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
        public void GetOrganizationByName_GivenNotPresentOrganizationName_ReturnNull()
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
        public void UpdateCountry_GivenUpdatedCountry_ReturnOkResultWithUpdatedCountry()
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
        public void UpdateCountry_GivenUpdatedCountryWithNotPresentId_ReturnOkResultWithUpdatedCountry()
        {
            //Arrange
            var country = MockDataProvider.GetMockedCountryModel();
            var updatedCountry = new CountryModel()
            {
                Id = Guid.NewGuid(),
                Name = "UpdatedModel",
                ContactEmail = "UpdatedContactEmail"
            };

            using var context = new CEContext(_dbContextOptions);
            context.Countries.Add(country);
            context.SaveChanges();

            using var newContext = new CEContext(_dbContextOptions);
            var sut = new AdministrativeEntitiesUOW(newContext);

            //Act
            var updateCountryTask = () => sut.UpdateCountry(updatedCountry);

            //Assert
            updateCountryTask.Should().Throw<Exception>();
        }

        [Test]
        public void UpdateOrganization_GivenUpdatedOrganization_ReturnOkResultWithUpdatedOrganization()
        {
            //Arrange
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var updatedOrganization = new OrganizationModel()
            {
                Id = organization.Id,
                TenantId = organization.TenantId,
                Name = "UpdatedModel",
                ContactEmail = "UpdatedContactEmail",
                Userquota = 558
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
            result!.Userquota.Should().Be(updatedOrganization.Userquota);
        }

        [Test]
        public void UpdateOrganization_GivenUpdatedOrganizationWithNewId_ReturnOkResultWithUpdatedOrganization()
        {
            //Arrange
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var updatedOrganization = new OrganizationModel()
            {
                Id = Guid.NewGuid(),
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
            var updateOrganizationTask = () => sut.UpdateOrganization(updatedOrganization);

            //Assert
            updateOrganizationTask.Should().Throw<Exception>();
        }

        [Test]
        public void DeleteOrganization_GivenOrganizationId_ReturnOkResult()
        {
            //Arrange
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var organization2 = new OrganizationModel()
            {
                ContactEmail = "ContactEmail",
                Name = "Organization2",
                TenantId = Guid.NewGuid().ToString(),
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
        public void DeleteOrganization_GivenInvalidOrganizationId_ReturnOkResult()
        {
            //Arrange
            var organizationIdToBeDeleted = Guid.NewGuid();
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var organization2 = new OrganizationModel()
            {
                ContactEmail = "ContactEmail",
                Name = "Organization2",
                TenantId = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid(),
            };

            _context.Organizations.Add(organization);
            _context.Organizations.Add(organization2);
            _context.SaveChanges();
            var sut = new AdministrativeEntitiesUOW(_context);

            var organizationEntities = _context.Organizations.Where(o => o != null);
            organizationEntities.Count().Should().Be(2);

            //Act
            var deleteOrganizationTask = () => sut.DeleteOrganization(organizationIdToBeDeleted);

            //Assert
            deleteOrganizationTask.Should().Throw<Exception>();
        }

        [Test]
        public void DeleteCountry_GivenUpdatedCountry_ReturnOkResultWithUpdatedCountry()
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
        public void DeleteCountry_GivenUpdatedCountryWithNewId_ReturnOkResultWithUpdatedCountry()
        {
            //Arrange
            var countryIdToBeDeleted = Guid.NewGuid();
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
            var deleteCountryTask = () => sut.DeleteCountry(countryIdToBeDeleted);

            //Assert
            deleteCountryTask.Should().Throw<Exception>();
        }

        [Test]
        public void GetCountries_GivenNoParameter_ReturnsListOfCountries()
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
        public void GetCountries_DBContainsNoCountry_ReturnsEmptyListOfCountries()
        {
            //Arrange
            var sut = new AdministrativeEntitiesUOW(_context);

            //Act
            var result = sut.GetCountries();

            //Assert
            result.Should().BeOfType(typeof(List<CountryModel>));
            var countryList = (List<CountryModel>)result;
            countryList.Count.Should().Be(0);
        }

        [Test]
        public void GetOrganizations_GivenNoParameter_ReturnsListOfOrganizations()
        {
            //Arrange
            var organization = MockDataProvider.GetMockedOrganizationModel();
            var organization2 = new OrganizationModel()
            {
                ContactEmail = "ContactEmail",
                Name = "organization2",
                Id = Guid.NewGuid(),
                TenantId = "tenantId",
                Userquota = 123
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
            organizationList.FirstOrDefault(x => x.TenantId == organization2.TenantId).Userquota.Should().Be(123);
        }

        [Test]
        public void GetOrganizations_DBContainsNoOrganization_ReturnsEmptyListOfOrganizations()
        {
            //Arrange
            var sut = new AdministrativeEntitiesUOW(_context);

            //Act
            var result = sut.GetOrganizations();

            //Assert
            result.Should().BeOfType(typeof(List<OrganizationModel>));
            var organizationList = (List<OrganizationModel>)result;
            organizationList.Count.Should().Be(0);
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

            var sut = new UserUOW(_context, _communicationService, _organisationUow);

            //Act
            var returnedUser = sut.GetUser(userId, userInfo);

            //Assert
            returnedUser.Should().NotBeNull();
            returnedUser.Should().BeEquivalentTo(mockedUser);
        }

        [Test]
        public void GetUser_GivenInvalidId_ExpectedNull()
        {
            //Arrange
            var userIdToBeFound = Guid.NewGuid().ToString();
            var mockedUser = MockDataProvider.GetMockedUserModel(); 
            var mockIds = MockDataProvider.GetUserIdInformationRecord();
            var userId = mockIds.UserId + 5674;
            mockedUser.UserId = userId;
            mockedUser.TenantID = mockIds.TenantId;

            _context.Users.Add(mockedUser);
            _context.SaveChanges();

            var userInfo = new UserIdsRecord() { Role = UserRole.User, UserId = mockedUser.UserId, TenantId = mockedUser.TenantID };

            var sut = new UserUOW(_context, _communicationService, null);

            //Act
            var returnedUser = sut.GetUser(userIdToBeFound, userInfo);

            //Assert
            returnedUser.Should().BeNull();
        }

        [Test]
        [TestCase(UserRole.SystemAdmin)] // isSystemAdmin = true, isTenantAdmin = false, isSelf = false
        [TestCase(UserRole.Admin)] // isSystemAdmin = false, isTenantAdmin = true, isSelf = false
        public void GetUser_GivenOtherUsersIdRoleIsSystemAdmin_ReturnedUserId(UserRole userRole)
        {
            //Arrange
            var mockedUser = MockDataProvider.GetMockedUserModel(); 
            var mockIds = MockDataProvider.GetUserIdInformationRecord();
            var userId = mockIds.UserId + 5674;
            var userRedcordUserId = userId + 1; //User is not self
            mockedUser.UserId = userId; 
            mockedUser.TenantID = mockIds.TenantId;

            _context.Users.Add(mockedUser);
            _context.SaveChanges();

            var userInfo = new UserIdsRecord() { Role = userRole, UserId = userRedcordUserId, TenantId = mockedUser.TenantID };

            var sut = new UserUOW(_context, _communicationService, _organisationUow);

            //Act
            var returnedUser = sut.GetUser(userId, userInfo);

            //Assert
            returnedUser.Should().NotBeNull();
            returnedUser.Should().BeEquivalentTo(mockedUser);
        }

        [Test]
        public void GetUser_GivenOtherUsersIdRoleIsAdminOfOtherTenant_ReturnedUserId()
        {
            //Arrange
            var mockedUser = MockDataProvider.GetMockedUserModel(); 
            var mockIds = MockDataProvider.GetUserIdInformationRecord();
            var userId = mockIds.UserId + 5674;
            var userRedcordUserId = userId + 1; //User is not self
            var adminTenantId = mockedUser.TenantID + 1;//User has other TenantId
            mockedUser.UserId = userId;
            mockedUser.TenantID = mockIds.TenantId; 
            
            _context.Users.Add(mockedUser);
            _context.SaveChanges();

            var userInfo = new UserIdsRecord() { Role = UserRole.Admin, UserId = userRedcordUserId, TenantId = adminTenantId };

            var sut = new UserUOW(_context, _communicationService, _organisationUow);

            //Act
            var returnedUser = sut.GetUser(userId, userInfo);

            //Assert
            returnedUser.Should().BeNull();
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
            var sut = new UserUOW(_context, _communicationService, _organisationUow);

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

        [Test]
        public void UpdateUser_GivenUserAndInvalidId_ExpectedReturnedUpdatedUser()
        {
            //Arrange
            var invalidUserId = Guid.NewGuid().ToString();
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
            var sut = new UserUOW(_context, _communicationService, null);

            var adminInfo = new UserIdsRecord() { UserId = "Admin1", TenantId = originalUser.TenantID, Role = UserRole.Admin };

            //Act
            var updateUserTask = () => sut.UpdateUser(invalidUserId, updatedUser, adminInfo);

            //Assert
            updateUserTask.Should().ThrowAsync<Exception>();
        }

        [Test]
        [TestCase(UserRole.SystemAdmin)] // isSystemAdmin = true, isTenantAdmin = false, isSelf = false
        [TestCase(UserRole.Admin)] // isSystemAdmin = false, isTenantAdmin = true, isSelf = false
        public async Task UpdateUser_GivenVariousAdminRoles_ExpectedReturnedUpdatedUser(UserRole userRole)
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

            var adminId = "Admin1"; //Admin has other UseriD

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
            var sut = new UserUOW(_context, _communicationService, _organisationUow);

            var adminInfo = new UserIdsRecord() { UserId = adminId, TenantId = originalUser.TenantID, Role = userRole};

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

        [Test]
        public async Task UpdateUser_GivenAdminUserOfOtherTenant_ExpectedException() //Role is Admin: WrongTenantID
        {
            //Arrange
            var originalUser = MockDataProvider.GetMockedUserModel(); 

            var newFirstName = "ChangedFirstName";
            var newSurname = "ChangedSurName";

            var newClinicalSetting = PatientDataEnums.ClinicalSetting.SecondaryCare;

            var newUserRole = UserRole.SystemAdmin;

            var expectedBiomarkerOrder = originalUser.BiomarkerOrders;

            var newBiomarkerOrder = new Collection<BiomarkerOrderModel> 
            {
                new() { OrderNumber = 1, BiomarkerId = "first", PreferredUnit = "unit", User = null, UserId = "id" },
                new() { OrderNumber = 2, BiomarkerId = "second", PreferredUnit = "unit", User = null, UserId = "id" }
            };

            var adminId = "Admin1"; //Admin has other UseriD
            var adminTenant = "OtherTenant";

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
            var sut = new UserUOW(_context, _communicationService, _organisationUow);

            var adminInfo = new UserIdsRecord() { UserId = adminId, TenantId = adminTenant, Role = UserRole.Admin };

            //Act
            var returnedUser = async () => await sut.UpdateUser(originalUser.UserId, updatedUser, adminInfo);

            //Assert
            await returnedUser.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task UpdateUser_GivenNoAdminUser_ExpectedReturnedUserWithOutChangedPrivilegedProperties() //Role is Admin: WrongTenantID
        {
            //Arrange
            var originalUser = MockDataProvider.GetMockedUserModel();

            var newFirstName = "ChangedFirstName";
            var newSurname = "ChangedSurName";

            var originalClinicalSetting = PatientDataEnums.ClinicalSetting.SecondaryCare;
            var newClinicalSetting = PatientDataEnums.ClinicalSetting.PrimaryCare;

            var originalUserRole = UserRole.Admin;
            var newUserRole = UserRole.SystemAdmin;

            var expectedBiomarkerOrder = originalUser.BiomarkerOrders;
            var newBiomarkerOrder = new Collection<BiomarkerOrderModel>
            {
                new() { OrderNumber = 2, BiomarkerId = "first", PreferredUnit = "unit", User = null, UserId = "id" },
                new() { OrderNumber = 1, BiomarkerId = "second", PreferredUnit = "unit", User = null, UserId = "id" }
            };

            originalUser.ClinicalSetting = originalClinicalSetting;
            originalUser.Role = originalUserRole;
            originalUser.BiomarkerOrders = expectedBiomarkerOrder;
            originalUser.TenantID = "tenant1";

            var adminId = originalUser.UserId; //Admin has same UseriD
            var adminTenant = originalUser.TenantID; //Admin has same TenantId as User

            _context.Users.Add(originalUser);
            _context.SaveChanges();

            var updatedUser = MockDataProvider.GetMockedUserModel(); 
            updatedUser.FirstName = newFirstName;
            updatedUser.Surname = newSurname;
            updatedUser.BiomarkerOrders = newBiomarkerOrder;
            updatedUser.Role = newUserRole;
            updatedUser.ClinicalSetting = newClinicalSetting;

            var sut = new UserUOW(_context, _communicationService, _organisationUow);

            var adminInfo = new UserIdsRecord() { UserId = adminId, TenantId = adminTenant, Role = UserRole.User };

            //Act
            var returnedUser = await sut.UpdateUser(originalUser.UserId, updatedUser, adminInfo);

            //Assert
            returnedUser.Should().NotBeNull();
            returnedUser.FirstName.Should().Be(newFirstName);
            returnedUser.Surname.Should().Be(newSurname);
            returnedUser.BiomarkerOrders.Should().NotBeNullOrEmpty();
            returnedUser.BiomarkerOrders.FirstOrDefault(x => x.BiomarkerId == "first").Should().NotBeNull();
            returnedUser.BiomarkerOrders.FirstOrDefault(x => x.BiomarkerId == "first")!.OrderNumber.Should().Be(1);
            returnedUser.ClinicalSetting.Should().Be(originalClinicalSetting);
            returnedUser.Role.Should().Be(originalUserRole);
        }
    }
}
