using CE_API_V2.Utility;

namespace CE_API_Test.UnitTests.Utility;

[TestFixture]
internal class TypeToFloatConverterTests
{

    [SetUp]
    public async Task Setup()
    {
    }

    [Test]
    [TestCase(true, 1f)]
    [TestCase(false, 0f)]
    public void MapBoolToFloat_GivenObject_CorrectFloatValue(bool boolValue, float expectedReturnValue)
    {
        //Arrange 

        //Act
        var result = TypeToFloatConverter.MapBoolToFloat(boolValue);

        //Assert
        result.Should().Be(expectedReturnValue);
    }

    [Test]
    [TestCase(TestEnum.Enum3, 2f)]
    [TestCase(TestEnum.Enum1, 0f)]
    [TestCase(TestEnum.Enum2, 1f)]
    public void MapEnumValueToFloat_GivenEnum_CorrectFloatValue(TestEnum enumValue, float expectedReturnValue)
    {
        //Arrange 

        //Act
        var result = TypeToFloatConverter.MapEnumValueToFloat(enumValue);

        //Assert
        result.Should().Be(expectedReturnValue);
    }

    internal enum TestEnum
    {
        Enum1,
        Enum2,
        Enum3,
    }
}