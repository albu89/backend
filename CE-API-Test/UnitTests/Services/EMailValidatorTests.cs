using CE_API_V2.Services;

namespace CE_API_Test.UnitTests.Services
{
    [TestFixture]
    public class EmailValidatorTests
    {
        [Test]
        [TestCase("mail@mail.mail", true)]
        [TestCase("mail.mail@mail.mail", true)]
        public void GetRequestAccessEmailConfiguration_GivenInvalidEmailFormat_ExpectedValidRequest(string email, bool expectedResult)
        {
            //Arrange
            var sut = new EmailValidator();

            //Act 
            var result = sut.EmailAddressIsValid(email);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        [TestCase("mail", false)]
        [TestCase("mail.mail", false)]
        [TestCase("mail.mail@mail@mail", false)]
        [TestCase("mail@mail.mail@mail", false)]
        public void GetRequestAccessEmailConfiguration_GivenValidEmailFormat_ExpectedInvalidResult(string email, bool expectedResult)
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