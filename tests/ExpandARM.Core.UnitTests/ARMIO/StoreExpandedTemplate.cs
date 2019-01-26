using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.IO.Abstractions;

namespace ExpandARM.Core.UnitTests
{
    [TestClass]
    public class StoreExpandedTemplate
    {
        private IFileSystem fileSystem;
        private IArmIO sut;

        [TestInitialize]
        public void Initialize()
        {
            fileSystem = MockFileSystemImpl.FileSystem;
            sut = ArmIO.Create(fileSystem);
        }

        [TestMethod, TestCategory("unit")]
        public void SaveExpandedTemplate_Should_Be_As_Expected_Template()
        {
            // Arrange
            var filePath = @"c:\templates\main\arm.template.with.templateLink.json";
            var arm = sut.LoadArmTemplate(filePath);
            sut.ExpandArmTemplate(arm);

            // Act
            var storedFileName = sut.SaveExpandedTemplate(arm);

            // Assert
            JObject contentInExpandedFile = JObject.Parse(fileSystem.File.ReadAllText(storedFileName));
            JObject expectedContent = JObject.Parse(fileSystem.File.ReadAllText(@"c:\templates\main\arm.expected.extended.template.json"));
            contentInExpandedFile.Should().BeEquivalentTo(expectedContent);
        }

        [TestMethod, TestCategory("unit")]
        public void SaveExpandedTemplate_ShouldHaveCorrectExpandedFilename()
        {
            // Arrange
            var filePath = @"c:\templates\main\arm.template.with.templateLink.json";
            var arm = sut.LoadArmTemplate(filePath);
            sut.ExpandArmTemplate(arm);

            // Act
            var storedFileName = sut.SaveExpandedTemplate(arm);

            // Assert
            storedFileName.Should().EndWith("expanded.json");
        }

        [TestMethod, TestCategory("unit")]
        public void SaveExpandedTemplate_IfOutputFilenameProived_ItIsUsed()
        {
            // Arrange
            var inputFilePath = @"c:\templates\main\arm.template.with.templateLink.json";
            var expectedOutputFilePath = @"c:\templates\main\arm.template.with.templateLink.processed.json";
            var arm = sut.LoadArmTemplate(inputFilePath);
            sut.ExpandArmTemplate(arm);

            // Act
            var storedFileName = sut.SaveExpandedTemplate(arm, expectedOutputFilePath);

            // Assert
            storedFileName.Should().Be(expectedOutputFilePath);
        }
    }
}
