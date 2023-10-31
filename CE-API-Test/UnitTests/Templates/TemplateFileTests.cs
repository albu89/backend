using CE_API_V2.Constants;
using Newtonsoft.Json.Linq;

namespace CE_API_Test.UnitTests.Templates
{
    [TestFixture]
    internal class TemplateFileTests
    {
        private string[] _files;
        private string _path;

        [SetUp]
        public void Setup()
        {
            _path = Path.Combine(LocalizationConstants.TemplatesSubpath);
            _files = Directory.GetFiles(_path);
        }

        [Test]
        public void ValidateJsonSchemaFiles_GivenFilesInFolder_ReturnValidSchemaResult()
        {
            //Arrange
            var jsonFiles = _files.Where(x => x.Contains(".json")).ToList();

            foreach (var file in jsonFiles)
            {
                //Act
                var jsonContent = File.ReadAllText(file);
                jsonContent.Should().NotBeNullOrEmpty();
                JToken jsonToken = JToken.Parse(jsonContent);

                //Assert
                if (jsonToken.Type is JTokenType.Array)
                {
                    var array = JArray.Parse(jsonContent);
                    array.Should().NotBeNullOrEmpty();
                }
                else if (jsonToken.Type is JTokenType.Object)
                {
                    var jObject = JObject.Parse(jsonContent);
                    jObject.Should().NotBeNullOrEmpty();
                }
                else
                {
                    Assert.Fail("JTokenType missing");
                }
            }
        }

        [Test]
        public void EnsureAllJsonFilesAreTested_GivenAllFilesInFolder_ReturnValidSchemaResult()
        {
            //Arrange

            var path = Path.Combine(LocalizationConstants.TemplatesSubpath);

            //Act
            var fileCount = _files.Length;
            var jsonFiles = _files
                .Where(file => file.EndsWith(".json"))
                .ToArray();
            int ignoredFilesCount = _files.Count(file => !file.EndsWith(".json")); // only json - files are tested

            //Assert
            jsonFiles.Should().NotBeNullOrEmpty();
            jsonFiles.Length.Should().Be(fileCount - ignoredFilesCount);
        }
    }
}
