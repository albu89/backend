using System.IO;
using System.Xml;
using CE_API_V2.Constants;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;


namespace CE_API_Test.UnitTests.Templates
{
    [TestFixture]
    internal class TemplateFileTests
    {
        private string[] _files;

        [SetUp]
        public void Setup()
        {
            var path = Path.Combine(LocalizationConstants.TemplatesSubpath);
            _files = Directory.GetFiles(path);
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
        public void ValidateHTMLSchemaFiles_GivenFilesInFolder_ReturnValidSchemaResult()
        {
            //Arrange
            var files = _files.Where(x => x.Contains("html")).ToList();

            //Act 

            //Assert
            //Todo 
        }

        [Test]
        public void EnsureAllFileTypesAreTested_GivenAllFilesInFolder_ReturnValidSchemaResult()
        {
            //Arrange

            var path = Path.Combine(LocalizationConstants.TemplatesSubpath);

            //Act
            var files = Directory.GetFiles(path);
            var fileCount = files.Length;
            string[] htmlAndJsonFiles = Directory.GetFiles(path)
                .Where(file => file.EndsWith(".html") || file.EndsWith(".json"))
                .ToArray();

            //Assert
            htmlAndJsonFiles.Should().NotBeNullOrEmpty();
            htmlAndJsonFiles.Length.Should().Be(fileCount);
        }
    }
}
