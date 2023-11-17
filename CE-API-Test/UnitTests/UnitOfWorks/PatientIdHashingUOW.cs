using CE_API_V2.Hasher;
using Microsoft.Extensions.Configuration;

namespace CE_API_Test.UnitTests.UnitOfWorks
{
    [TestFixture]
    internal class PatientIdHashingUOWTests
    {
        private IConfiguration _configuration;

        #region Setup

        [SetUp]
        public void SetUp()
        {
            var testConfig = new Dictionary<string, string?>()
            {
                { "Salt", "123456789SaltValue" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfig)
                .Build();
        }

        #endregion

        #region TearDown

        [TearDown]
        public void TearDown()
        {

        }

        #endregion


        [Test]
        [TestCase("firstName", "lastName", 1, 1, 2001, "xi9BHA3jjNy5Vnd+3mofhY8VVKZozcFNlVORJYQ4W28OzUbgrOp8bjUMPRxrI0mQ5bY3Lj9O7zKVbbxACTac8w==")]
        [TestCase("otherFirstName", "otherLastName", 1, 1, 2001,"OLDgghTzHgD37om4CKA8IoXQlG6BZE+Fdc4K+KBttkemMuK/2+rXZLHd5qiJlUYu1+jo1kyttJy+XxhAUA9PYg==")]
        [TestCase("firstName", "lastName", 1, 1, 2022, "paaoV5a4s+YkzbOxhqTtUokVcS9DnZa+RsdbFWUQVqoW1+eG1K5EHlosPHYHInCB/Q+p/V1qkUbhGZuwEZDhTQ==")]
        [TestCase("firstName", "lastName", 1, 2, 2001, "QrTHLWDoiC17VtWX5CXD8QLsyffR7sWRK0kZIlGVwtLgZFjUvt2oMGxNRYHVJxYUoLkQzxs4sfYVupmwQg7DFg==")]
        [TestCase("firstName", "lastName", 2, 1, 2001, "6Z00pWxvP/eAA8YOYucp1HgBO4A3WivJ6TaaWuG7q7BYp/s2khrQ2S8/RFhhQS/z6KbfyJFWZd9o+wrRgOTUoA==")]
        [TestCase("", "lastName", 1, 1, 2001, "1edPvxX7G7s4R78ss3PKXli4LOg1RG60Wsla36SbJ1B9gbjnjyMxwx6e7hxn1YO7tWxvXlPWjLTWBykFSJt0mA==")]
        [TestCase("firstName", "", 1, 1, 2001, "m+SPBH05EWnXZfUPRqfHRH39/H2txMHHxpPcwA/28MFJCCaj6iaM24hjJZQPwnFwxgjy2bmOvEtnIXDDGNlHug==")]
        public void HashPatientId_GivenValidPatientData_ReturnHashValue(string firstname, string lastName, int day, int month, int year, string expectedHash)
        {
            //Arrange
            DateTime date = new DateTime(year, month, day);

            var sut = new PatientIdHashingUOW(_configuration);

            //Act
            var result = sut.HashPatientId(firstname, lastName, date);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedHash);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void HashPatientId_GivenEmptySaltValues_ThrowsException(string? salt)
        {
            //Arrange
            var testConfig = new Dictionary<string, string?>()
            {
                { "Salt", salt }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfig)
                .Build();

            DateTime date = new DateTime(2001, 1, 1);
            var sut = new PatientIdHashingUOW(_configuration);

            //Act
            var task = () => sut.HashPatientId("firstName", "lastName", date);

            //Assert
            task.Should().NotBeNull();
            task.Should().Throw<NotImplementedException>();
        }
    }
}
