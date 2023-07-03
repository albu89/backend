using CE_API_V2.Models;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Moq;
using System.Security.Claims;
using CE_API_V2.Models.DTO;
using CE_API_Test.TestUtilities;
using CE_API_V2.UnitOfWorks;
using AutoMapper;
using CE_API_V2.Data;
using CE_API_V2.Models.Mapping;
using Microsoft.EntityFrameworkCore;
using CE_API_V2.Models.Records;

namespace CE_API_Test.UnitTests.UnitOfWorks
{
    [TestFixture]
    public class UserUOWTests
    {
        private IInputValidationService _inputValidationService;
        private IUserUOW _userUOW;
        private IUserInformationExtractor _userInformationExtractor;
        private IMapper _mapper;
        private CEContext _ceContext;

        [SetUp]
        public void SetUp()
        {
            var inputValidationServiceMock = new Mock<IInputValidationService>();
            var userUOWMock = new Mock<IUserUOW>();
            var userInfoExtractorMock = new Mock<IUserInformationExtractor>();

            inputValidationServiceMock.Setup(x => x.ValidateUser(It.IsAny<CreateUserDto>())).Returns(true);
            userUOWMock.Setup(x => x.StoreUser(It.IsAny<CreateUserDto>(), It.IsAny<UserIdsRecord>())).Returns(Task.FromResult(new User()));
            userInfoExtractorMock.Setup(x => x.GetUserIdInformation(It.IsAny<ClaimsPrincipal>())).Returns(new UserIdsRecord());

            _inputValidationService = inputValidationServiceMock.Object;
            _userUOW = userUOWMock.Object;
            _userInformationExtractor = userInfoExtractorMock.Object;

            var options = new DbContextOptionsBuilder<CEContext>()
                .UseInMemoryDatabase(databaseName: "CEDatabase")
                .Options;

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();
            _ceContext = new CEContext(options);
        }


        [Test]
        public async Task CreatedUser_GivenMockedUserDto_ReturnOkResult()
        {
            //Arrange
            var sut = new UserUOW(_ceContext, _mapper);
            var userDto = MockDataProvider.GetMockedCreateUserDto();
            var userIdInfoRecord = MockDataProvider.GetUserIdInformationRecord();

            //Act
            var result = sut.StoreUser(userDto, userIdInfoRecord);

            //Assert
            result.Should().NotBeNull();
        }

    }
}
