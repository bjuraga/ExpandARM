using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.IO.Abstractions;

namespace ExpandARM.Core.UnitTests
{
    [TestClass]
    public class LoadArmTemplate
    {
        private IFileSystem fileSystem;
        private IArmIO sut;

        [TestInitialize]
        public void Initialize()
        {
            fileSystem = MockFileSystemImpl.FileSystem;
            sut = ArmIO.Create(fileSystem);
        }

        [TestMethod]
        public void LoadArmTemplate_MinimalFile_Returns_ExpectedArmTemplate()
        {
            // Arrange
            var filePath = @"c:\templates\main\minimal.arm.template.json";
            var expectedArmTemplate = new ArmTemplate(filePath, JObject.Parse(fileSystem.File.ReadAllText(filePath)));

            // Act
            var arm = sut.LoadArmTemplate(filePath);

            // Assert
            arm.Should().BeEquivalentTo(expectedArmTemplate);
        }

        [TestMethod]
        public void LoadArmTemplate_MinimalFile_Contains_resources_array()
        {
            // Arrange
            var filePath = @"c:\templates\main\minimal.arm.template.json";
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            var resourcesType = (Type)((dynamic)arm.OriginalContent).resources.GetType();

            // Assert
            resourcesType.Should().Be(typeof(JArray));
        }

        [TestMethod]
        public void LoadArmTemplate_MinimalFileWithoutTemplateLink_Returns_ArmTemplate_With_NeedsExpansion_False()
        {
            // Arrange
            var filePath = @"c:\templates\main\minimal.arm.template.json";

            // Act
            var arm = sut.LoadArmTemplate(filePath);

            // Assert
            arm.NeedsExpansion.Should().Be(false);
        }

        [TestMethod]
        public void LoadArmTemplate_WithTemplateLink_Returns_ArmTemplate_With_NeedsExpansion_True()
        {
            // Arrange
            var filePath = @"c:\templates\main\arm.template.with.templateLink.json";

            // Act
            var arm = sut.LoadArmTemplate(filePath);

            // Assert
            arm.NeedsExpansion.Should().Be(true);
        }

        [TestMethod]
        public void LoadArmTemplate_MinimalFileByRelativePath_Returns_ArmTemplate_With_Expected_Contents()
        {
            // Arrange
            // simulate working directory is set to C:\templates\main\
            fileSystem.Directory.SetCurrentDirectory(@"c:\templates\main\");
            var filePath = @"minimal.arm.template.json";

            // Act
            var arm = sut.LoadArmTemplate(filePath);

            // Assert
            fileSystem.Path.IsPathRooted(arm.FilePath).Should().BeTrue();
        }
    }
}
