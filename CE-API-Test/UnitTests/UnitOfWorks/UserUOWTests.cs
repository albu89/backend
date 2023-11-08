using System.Collections.ObjectModel;
using CE_API_V2.Data;
using CE_API_V2.Models;
using CE_API_V2.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Moq;
using System.Security.Claims;
using CE_API_V2.Models.DTO;
using CE_API_Test.TestUtilities;
using CE_API_V2.Models.Records;
using Azure.Communication.Email;
using CE_API_V2.Models.Enum;
using CE_API_V2.Data.Repositories.Interfaces;
using System.Linq.Expressions;

namespace CE_API_Test.UnitTests.UnitOfWorks
{
    [TestFixture]
    internal class UserUOWTests
    {
        private DbContextOptions<CEContext> _dbContextOptions;
        private CEContext _context;
        private DbSet<BiomarkerOrderModel> _dbSet;
        private IUserUOW _userUOW;
        private IOrganisationUOW _orgaUOW;
        private IGenericRepository<UserModel> _userRepo;
        private ICommunicationService _communicationService;
        private static readonly BiomarkerOrderModel insertBiomarkerOrder = new() { BiomarkerId = "age", UserId = "TestUser", OrderNumber = 1, PreferredUnit = "SI" };
        private static readonly BiomarkerOrderModel updateBiomarkerOrder = new() { BiomarkerId = "age", UserId = "TestUser", OrderNumber = 2, PreferredUnit = "SI" };

        #region Setup

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CEContext>().UseInMemoryDatabase(databaseName: "CEUnitTestDb").Options;

            _context = new CEContext(_dbContextOptions);
            _dbSet = _context.Set<BiomarkerOrderModel>();

            var inputValidationServiceMock = new Mock<IInputValidationService>();
            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);

            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();

            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);
            userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(new UserIdsRecord());
            userInfoExtractorMock.Setup(x
                => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(new UserIdsRecord());
                

            var communicationService = new Mock<ICommunicationService>();
            communicationService.Setup(x
                => x.SendAccessRequest(It.IsAny<AccessRequest>())).Returns(Task.FromResult(EmailSendStatus.Succeeded));
            _orgaUOW = new Mock<IOrganisationUOW>().Object;
            _userRepo = new Mock<IGenericRepository<UserModel>>().Object;

            _communicationService = communicationService.Object;
            _userUOW = new UserUOW(_context, _communicationService, _orgaUOW);
        }

        #endregion

        #region StoreBiomarkerOrder

        [Test]
        public async Task StoreBiomarkerOrder_Receives_Db_Call()
        {
            await _userUOW.StoreOrEditBiomarkerOrder(new BiomarkerOrderModel[] { insertBiomarkerOrder }, "TestUser");

            var addedEntity = _dbSet.Find(insertBiomarkerOrder.UserId, insertBiomarkerOrder.BiomarkerId);
            Assert.That(addedEntity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(addedEntity.BiomarkerId, Is.EqualTo(insertBiomarkerOrder.BiomarkerId));
                Assert.That(addedEntity.UserId, Is.EqualTo(insertBiomarkerOrder.UserId));
                Assert.That(addedEntity.OrderNumber, Is.EqualTo(insertBiomarkerOrder.OrderNumber));
                Assert.That(addedEntity.PreferredUnit, Is.EqualTo(insertBiomarkerOrder.PreferredUnit));
            });
        }

        #endregion

        #region EditBiomarkerOrder

        [Test]
        public async Task EditBiomarkerOrder_Receives_Db_Call()
        {
            _dbSet.Add(insertBiomarkerOrder);
            _context.SaveChanges();

            await _userUOW.StoreOrEditBiomarkerOrder(new[] { updateBiomarkerOrder }, "TestUser");

            var updatedEntity = _dbSet.Find(updateBiomarkerOrder.UserId, updateBiomarkerOrder.BiomarkerId);
            Assert.That(updatedEntity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(updatedEntity.BiomarkerId, Is.EqualTo(updateBiomarkerOrder.BiomarkerId));
                Assert.That(updatedEntity.UserId, Is.EqualTo(updateBiomarkerOrder.UserId));
                Assert.That(updatedEntity.OrderNumber, Is.EqualTo(updateBiomarkerOrder.OrderNumber));
                Assert.That(updatedEntity.PreferredUnit, Is.EqualTo(updateBiomarkerOrder.PreferredUnit));
            });
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
        public async Task IsActive_State_NotModifiable()
        {
            var organisationModel = new OrganizationModel() { TenantId = "MockedTenantId", Userquota = 2 };
            var mockedOrgaUOW = new Mock<IOrganisationUOW>();
            mockedOrgaUOW.Setup(x => x.GetOrganisationWithTenantID("MockedTenantId")).Returns(organisationModel);
            var orgaUOW = mockedOrgaUOW.Object;

            var userList = new List<UserModel>()
            {
                MockDataProvider.GetMockedUserModel(),
                MockDataProvider.GetMockedUserModel()
        };
            var userIdInfoRecord = MockDataProvider.GetUserIdInformationRecord();

            var userRepo = new Mock<IGenericRepository<UserModel>>() { CallBase = true };
            userRepo.Setup(x => x.Get(It.IsAny<Expression<Func<UserModel, bool>>>(), null, ""))
                .Returns(userList);

            var useruow = new Mock<UserUOW>(_context, _communicationService, orgaUOW) { CallBase = true };

            var sut = useruow.Object;
            sut.UserRepository = userRepo.Object;

            var result = sut.CheckIfIsActiveStateIsModifiable(userIdInfoRecord);

            result.Should().BeFalse();
        }

        [Test]
        public async Task IsActive_State_Modifiable()
        {
            var organisationModel = new CE_API_V2.Models.OrganizationModel() { TenantId = "MockedTenantId", Userquota = 2 };
            var mockedOrgaUOW = new Mock<IOrganisationUOW>();
            mockedOrgaUOW.Setup(x => x.GetOrganisationWithTenantID("MockedTenantId")).Returns(organisationModel);
            var orgaUOW = mockedOrgaUOW.Object;

            var userList = new List<UserModel>()
            {
                MockDataProvider.GetMockedUserModel()
        };
            var userIdInfoRecord = MockDataProvider.GetUserIdInformationRecord();

            var userRepo = new Mock<IGenericRepository<UserModel>>() { CallBase = true };
            userRepo.Setup(x => x.Get(It.IsAny<Expression<Func<UserModel, bool>>>(), null, ""))
                .Returns(userList);

            var useruow = new Mock<UserUOW>(_context, _communicationService, orgaUOW) { CallBase = true };

            var sut = useruow.Object;
            sut.UserRepository = userRepo.Object;

            var result = sut.CheckIfIsActiveStateIsModifiable(userIdInfoRecord);

            result.Should().BeTrue();
        }

        [Test]
        public async Task CreateUser_GivenValidUser_ReturnOkResult()
        {
            //Arrange
            var sut = new UserUOW(_context, _communicationService, _orgaUOW);
            var user = MockDataProvider.GetMockedUserModel();
            var userIdInfoRecord = MockDataProvider.GetUserIdInformationRecord();

            user.UserId = userIdInfoRecord.UserId;
            user.TenantID = userIdInfoRecord.TenantId;

            //Act
            var result = await sut.StoreUser(user);

            //Assert
            result.Should().NotBeNull();
            result.TenantID.Should().Be(userIdInfoRecord.TenantId);
            result.UserId.Should().Be(userIdInfoRecord.UserId);
        }

        [Test]
        public async Task AccessRequest_GivenValidData_ReturnOkResult()
        {
            //Arrange
            var sut = new UserUOW(_context, _communicationService, _orgaUOW);
            var accessDto = MockDataProvider.GetMockedAccessRequestDto();

            //Act
            var result = await sut.ProcessAccessRequest(accessDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(EmailSendStatus.Succeeded);
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

            var userInfo = new UserIdsRecord() { UserId = userId, TenantId = "A", Role = UserRole.User };

            var sut = new UserUOW(_context, _communicationService, _orgaUOW);

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
            
            _context.Users.Add(originalUser);
            _context.SaveChanges();

            var updatedUser = MockDataProvider.GetMockedUserModel();
            updatedUser.FirstName = newFirstName;
            updatedUser.Surname = newSurname;
            updatedUser.BiomarkerOrders = newBiomarkerOrder;
            var sut = new UserUOW(_context, _communicationService, _orgaUOW);

            var userInfo = new UserIdsRecord() { UserId = updatedUser.UserId, Role = UserRole.User };

            //Act
            var returnedUser = await sut.UpdateUser(originalUser.UserId, updatedUser, userInfo);

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
        public async Task UpdateUser_GivenTenantAdmin_UpdatesIsActiveAndClinicalSetting()
        {
            //Arrange
            var originalUser = MockDataProvider.GetMockedUserModel();

            originalUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.PrimaryCare;
            originalUser.IsActive = false;
            
            _context.Users.Add(originalUser);
            _context.SaveChanges();

            var updatedUser = MockDataProvider.GetMockedUserModel();
            updatedUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.SecondaryCare;
            updatedUser.IsActive = true;
            var sut = new UserUOW(_context, _communicationService, _orgaUOW);
            
            var userInfo = new UserIdsRecord() { UserId = updatedUser.UserId + 1, Role = UserRole.Admin, TenantId = updatedUser.TenantID};


            //Act
            var returnedUser = await sut.UpdateUser(originalUser.UserId, updatedUser, userInfo);

            //Assert
            returnedUser.Should().NotBeNull();
            returnedUser.FirstName.Should().Be(updatedUser.FirstName);
            returnedUser.Surname.Should().Be(updatedUser.Surname);
            returnedUser.BiomarkerOrders.Should().NotBeNullOrEmpty();
            returnedUser.ClinicalSetting.Should().Be(PatientDataEnums.ClinicalSetting.SecondaryCare);
        }
        
        
        [Test]
        public async Task UpdateUser_GivenWrongTenantAdmin_NotUpdateIsActiveAndClinicalSetting()
        {
            //Arrange
            var originalUser = MockDataProvider.GetMockedUserModel();

            originalUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.PrimaryCare;
            originalUser.IsActive = false;
            
            _context.Users.Add(originalUser);
            _context.SaveChanges();

            var updatedUser = MockDataProvider.GetMockedUserModel();
            updatedUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.SecondaryCare;
            updatedUser.IsActive = true;
            var sut = new UserUOW(_context, _communicationService, _orgaUOW);
            
            var userInfo = new UserIdsRecord() { UserId = updatedUser.UserId + 1, Role = UserRole.Admin, TenantId = updatedUser.TenantID + "A"};


            //Act
            var returnedUserTask = async() => await sut.UpdateUser(originalUser.UserId, updatedUser, userInfo);

            //Assert
            returnedUserTask.Should().ThrowAsync<Exception>();
        }
        
        [Test]
        public async Task UpdateUser_GivenSystemAdmin_UpdatesIsActiveAndClinicalSetting()
        {
            //Arrange
            var originalUser = MockDataProvider.GetMockedUserModel();

            originalUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.PrimaryCare;
            originalUser.IsActive = false;
            
            _context.Users.Add(originalUser);
            _context.SaveChanges();

            var updatedUser = MockDataProvider.GetMockedUserModel();
            updatedUser.ClinicalSetting = PatientDataEnums.ClinicalSetting.SecondaryCare;
            updatedUser.IsActive = true;
            var sut = new UserUOW(_context, _communicationService, _orgaUOW);
            
            // User with wrong TenantId but SystemAdmin
            var userInfo = new UserIdsRecord() { UserId = updatedUser.UserId + 1, Role = UserRole.SystemAdmin, TenantId = updatedUser.TenantID + "A"};


            //Act
            var returnedUser = await sut.UpdateUser(originalUser.UserId, updatedUser, userInfo);

            //Assert
            returnedUser.Should().NotBeNull();
            returnedUser.FirstName.Should().Be(updatedUser.FirstName);
            returnedUser.Surname.Should().Be(updatedUser.Surname);
            returnedUser.BiomarkerOrders.Should().NotBeNullOrEmpty();
            returnedUser.ClinicalSetting.Should().Be(PatientDataEnums.ClinicalSetting.SecondaryCare);
        }

        [Test]
        public async Task GetUsers_ReturnsOnlyUsersFromSameTenant()
        {
            // Arrange
            var tenantId1 = "Tenant1";
            var tenantId2 = "Tenant2";
            
            var originalUser = MockDataProvider.GetMockedUserModel();
            originalUser.UserId = tenantId1 + "User";
            originalUser.TenantID = tenantId1;
            originalUser.BiomarkerOrders = null;
            _context.Users.Add(originalUser);
            await _context.SaveChangesAsync();
            originalUser.UserId = tenantId2 + "User";
            originalUser.TenantID = tenantId2;
            _context.Users.Add(originalUser);
            await _context.SaveChangesAsync();
            originalUser.UserId = tenantId2 + "User2";
            originalUser.TenantID = tenantId2;
            _context.Users.Add(originalUser);
            await _context.SaveChangesAsync();
            
            var sut = new UserUOW(_context, _communicationService, _orgaUOW);

            var userInfo = new UserIdsRecord()
            {
                TenantId = tenantId2,
                UserId = tenantId2 + "Admin",
                Role = UserRole.Admin
            };
            
            // Act
            var result = sut.GetUsersForAdmin(userInfo);

            result.Should().NotBeNull();
            result.Count().Should().Be(2, $"Tenant: {tenantId2} should have 2 users registered");
            result.Any(x => x.TenantID != tenantId2).Should().BeFalse("the method should not return any users from a different tenant");
        }
        
        [Test]
        public async Task GetUsers_ReturnsAllUsersForSystemAdmin()
        {
            // Arrange
            var tenantId1 = "Tenant1";
            var tenantId2 = "Tenant2";
            
            var originalUser = MockDataProvider.GetMockedUserModel();
            originalUser.UserId = tenantId1 + "User";
            originalUser.TenantID = tenantId1;
            originalUser.BiomarkerOrders = null;
            _context.Users.Add(originalUser);
            await _context.SaveChangesAsync();

            originalUser.UserId = tenantId2 + "User";
            originalUser.TenantID = tenantId2;
            _context.Users.Add(originalUser);
            await _context.SaveChangesAsync();

            originalUser.UserId = tenantId2 + "User2";
            originalUser.TenantID = tenantId2;
            _context.Users.Add(originalUser);
            await _context.SaveChangesAsync();
            
            var sut = new UserUOW(_context, _communicationService, _orgaUOW);

            var userInfo = new UserIdsRecord()
            {
                TenantId = tenantId2,
                UserId = tenantId2 + "Admin",
                Role = UserRole.SystemAdmin
            };
            
            // Act
            var result = sut.GetUsersForAdmin(userInfo);

            result.Should().NotBeNull();
            result.Count().Should().Be(3, $"SystemAdmins should see all 3 registered users");
        }
    }
}
