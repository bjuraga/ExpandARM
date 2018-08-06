using FluentAssertions;
using MergeARM.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace MergeARM.Tests.Core.ARMIO
{
    [TestClass]
    public class ArmIO_ExpandArmTemplate
    {
        [TestMethod]
        public void ExpandArmTemplate_Sets_IsExpanded_To_True()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\arm.template.with.templateLink.json";
            var sut = ArmIO.Create(fileSystem);
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            sut.ExpandArmTemplate(arm);

            // Assert
            arm.IsExpanded.Should().Be(true);
        }

        [TestMethod]
        public void ExpandArmTemplate_Produces_Template_Objects()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\arm.template.with.templateLink.json";
            var sut = ArmIO.Create(fileSystem);
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            sut.ExpandArmTemplate(arm);

            // Assert
            arm.ExpandedContent.SelectTokens("$..template").Any().Should().Be(true);
        }

        [TestMethod]
        public void ExpandArmTemplate_Template_Contains_FileContents_Of_LinkedTemplate()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\arm.template.with.templateLink.json";
            var expectedTemplateContent = JObject.Parse(fileSystem.File.ReadAllText(@"c:\reusable.templates\arm.linked.minimal.template.json"));
            var sut = ArmIO.Create(fileSystem);
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            sut.ExpandArmTemplate(arm);

            // Assert
            arm.ExpandedContent.SelectToken("$..template").ToString().Should().BeEquivalentTo(expectedTemplateContent.ToString());
        }

        [TestMethod]
        public void ExpandArmTemplate_Supports_FilePaths_With_Forward_Slash()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\arm.template.with.forward.slash.templateLink.json";
            var expectedTemplateContent = JObject.Parse(fileSystem.File.ReadAllText(@"c:\reusable.templates\arm.linked.minimal.template.json"));
            var sut = ArmIO.Create(fileSystem);
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            sut.ExpandArmTemplate(arm);

            // Assert
            arm.ExpandedContent.SelectToken("$..template").ToString().Should().BeEquivalentTo(expectedTemplateContent.ToString());
        }
    }
}
