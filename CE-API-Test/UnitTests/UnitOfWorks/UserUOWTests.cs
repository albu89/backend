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
using AutoMapper;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Models.Records;
using Azure.Communication.Email;

namespace CE_API_Test.UnitTests.UnitOfWorks
{
    [TestFixture]
    internal class UserUOWTests
    {
        private DbContextOptions<CEContext> _dbContextOptions;
        private CEContext _context;
        private DbSet<BiomarkerOrderModel> _dbSet;
        private IUserUOW _userUOW;
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
            
            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();

            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUser>())).Returns(true);
            userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(new UserIdsRecord());
            var communicationService = new Mock<ICommunicationService>();
            userInfoExtractorMock.Setup(x 
                => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(new UserIdsRecord());
            communicationService.Setup(x 
                => x.SendAccessRequest(It.IsAny<AccessRequest>())).Returns(Task.FromResult(EmailSendStatus.Succeeded));
                
            _communicationService = communicationService.Object;

            _userUOW = new UserUOW(_context, _communicationService);
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
        }

        #endregion

        #region StoreBiomarkerOrder

        [Test]
        public async Task StoreBiomarkerOrder_Receives_Db_Call()
        {
            await _userUOW.StoreOrEditBiomarkerOrder(new BiomarkerOrderModel[] {insertBiomarkerOrder}, "TestUser");

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

            await _userUOW.StoreOrEditBiomarkerOrder(new [] {updateBiomarkerOrder}, "TestUser");

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
        public async Task CreatedUser_GivenMockedUserDto_ReturnOkResult()
        {
            //Arrange
            var sut = new UserUOW(_context, _communicationService);
            var user = MockDataProvider.GetMockedUser();
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
        public async Task ProcessCreationRequest_GivenMockedAccessDto_ReturnOkResult()
        {
            //Arrange
            var sut = new UserUOW(_context, _communicationService);
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
            var mockedUser = MockDataProvider.GetMockedUser();
            var mockIds = MockDataProvider.GetUserIdInformationRecord();
            var userId = mockIds.UserId + 5674;
            mockedUser.UserId = userId;
            mockedUser.TenantID = mockIds.TenantId;
                
            _context.Users.Add(mockedUser);
            _context.SaveChanges();

            var sut = new UserUOW(_context, _communicationService);
            
            //Act
            var returnedUser = sut.GetUser(userId);

            //Assert
            returnedUser.Should().NotBeNull();
            returnedUser.Should().BeEquivalentTo(mockedUser);
        }
    }
}
