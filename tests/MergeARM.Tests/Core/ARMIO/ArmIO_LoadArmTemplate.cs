using FluentAssertions;
using MergeARM.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;

namespace MergeARM.Tests.Core.ARMIO
{
    [TestClass]
    public class ArmIO_LoadArmTemplate
    {
        [TestMethod]
        public void LoadArmTemplate_MinimalFile_Returns_ArmTemplate()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\minimal.arm.template.json";
            var sut = ArmIO.Create(fileSystem);
            var expectedArmTemplate = new ArmTemplate(filePath, fileSystem.File.ReadAllText(filePath));

            // Act
            var arm = sut.LoadArmTemplate(filePath);

            // Assert
            arm.Should().BeEquivalentTo(expectedArmTemplate);
        }

        [TestMethod]
        public void LoadArmTemplate_MinimalFile_Contains_resources_array()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\minimal.arm.template.json";
            var sut = ArmIO.Create(fileSystem);
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
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\minimal.arm.template.json";
            var sut = ArmIO.Create(fileSystem);

            // Act
            var arm = sut.LoadArmTemplate(filePath);

            // Assert
            arm.NeedsExpansion.Should().Be(false);
        }

        [TestMethod]
        public void LoadArmTemplate_WithTemplateLink_Returns_ArmTemplate_With_NeedsExpansion_True()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\arm.template.with.templateLink.json";
            var sut = ArmIO.Create(fileSystem);

            // Act
            var arm = sut.LoadArmTemplate(filePath);

            // Assert
            arm.NeedsExpansion.Should().Be(true);
        }
    }
}
