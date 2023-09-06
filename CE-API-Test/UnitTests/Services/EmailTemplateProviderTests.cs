using CE_API_V2.Services;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class EmailTemplateProviderTests
    {
        [SetUp]
        public void SetUp()
        {

        }
        
        [Test]
        public void GetRequestBodyTemplate_ReturnOkResult()
        {
            //Arrange
            var sut = new EmailTemplateProvider();

            //Act
            var result = sut.GetRequestBodyTemplate();

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().ContainAll("{{{FirstName}}}", "{{{LastName}}}", "{{{PhoneNumber}}}", "{{{EmailAddress}}}");
        }

        [Test]
        public void GetActivateUserBodyTemplate_ReturnOkResult()
        {
            //Arrange
            var sut = new EmailTemplateProvider();

            //Act
            var result = sut.GetActivateUserBodyTemplate();

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().ContainAll("{{{FirstName}}}", "{{{LastName}}}", "{{{Id}}}");
        }
    }
}
