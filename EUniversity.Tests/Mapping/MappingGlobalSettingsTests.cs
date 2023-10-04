using EUniversity.Core.Mapping;
using Mapster;

namespace EUniversity.Tests.Maps
{
    public class MappingGlobalSettingsTests
    {
        private class Poco
        {
            public string? NullableString { get; set; }
        }

        private class Dto
        {
            public string? NullableString { get; set; }
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            MappingGlobalSettings.Apply();
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\t")]
        [TestCase("\r")]
        public void NullableString_EmptyOrWhiteSpace_MapsToNull(string testCase)
        {
            // Arrange
            Poco poco = new() { NullableString = testCase };

            // Act
            var dto = poco.Adapt<Dto>();

            // Assert
            Assert.That(dto.NullableString, Is.Null);
        }
    }
}
