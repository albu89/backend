using CE_API_V2.Services;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class EmailValidatorTests
    {
        [Test]
        [TestCase("mail", false)]
        [TestCase("mail@mail.mail", true)]
        [TestCase("mail.mail@mail.mail", true)]
        public async Task GetRequestAccessEmailConfiguration_GivenAccessDto_ExpectedProcessedRequest(string email, bool expectedResult)
        {
            //Arrange
            var sut = new EmailValidator();

            //Act 
            var result = sut.EmailAddressIsValid(email);

            //Assert
            result.Should().Be(expectedResult);
        }
    }
}