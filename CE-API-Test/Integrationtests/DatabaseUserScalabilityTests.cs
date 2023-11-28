using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Data;
using Microsoft.EntityFrameworkCore;

namespace CE_API_Test.Integrationtests;

[TestFixture]
internal class DatabaseUserScalabilityTests
{
    private const string DatabaseName = "TestCEContext";
    
    private DbContextOptions<CEContext> _options;
    private CEContext _context;

    [SetUp]
    public void DatabaseUserScalabilityTests_Setup()
    {
        _options = new DbContextOptionsBuilder<CEContext>().UseInMemoryDatabase(DatabaseName).Options;
        _context = new CEContext(_options);
    }
    
    [Test]
    public async Task Database_UserScalability_ShouldBeAbleToHandle1000Users()
    {
        //Arrange
        var expectedUserCount = 1000;
        var isActive = true;
        
        //Act
        for (int i = 0; i < 1000; i++)
        {
            _context.Users.Add(CreateUserModel(isActive, i));
        }

        _= _context.SaveChangesAsync();
        
        //Assert
        _context.Users.Count().Should().BeGreaterOrEqualTo(expectedUserCount);
    }

    [TearDown]
    public void DatabaseUserScalabilityTests_TearDown()
    {
        _context.Dispose();
    }
    
    private UserModel CreateUserModel(bool isActive, int counter)
    {
        var user = MockDataProvider.GetMockedUserModel();
        user.UserId = $"UserId_{counter}";
        user.IsActive = isActive;
        user.BiomarkerOrders = null;

        return user;
    }
}
