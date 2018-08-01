using FluentAssertions;
using MergeARM.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;

namespace MergeARM.Tests.Core.ARMIO
{
    [TestClass]
    public class ARMIO_Tests
    {
        [TestMethod]
        public void LoadArmTemplate_MinimalFile_Returns_ArmTemplate()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\minimal.arm.template.json";
            var sut = new ArmIO(fileSystem);
            var expectedArmTemplate = new ArmTemplate(filePath, fileSystem.File.ReadAllText(filePath));

            var arm = sut.LoadArmTemplate(filePath);

            arm.Should().BeEquivalentTo(expectedArmTemplate);
        }

        [TestMethod]
        public void LoadArmTemplate_MinimalFile_Contains_resources_array()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\minimal.arm.template.json";
            var sut = new ArmIO(fileSystem);
            var expectedArmTemplate = new ArmTemplate(filePath, fileSystem.File.ReadAllText(filePath));

            var arm = sut.LoadArmTemplate(filePath);

            var resourcesType = (Type)arm.OriginalContent.resources.GetType();
            resourcesType.Should().Be(typeof(JArray));
        }

        [TestMethod]
        public void LoadArmTemplate_MinimalFileWithoutTemplateLink_Returns_ArmTemplate_With_NeedsExpansion_False()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\minimal.arm.template.json";
            var sut = new ArmIO(fileSystem);

            var arm = sut.LoadArmTemplate(filePath);

            arm.NeedsExpansion.Should().Be(false);
        }

        [TestMethod]
        public void LoadArmTemplate_WithTemplateLink_Returns_ArmTemplate_With_NeedsExpansion_True()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\arm.template.with.templateLink.json";
            var sut = new ArmIO(fileSystem);

            var arm = sut.LoadArmTemplate(filePath);

            arm.NeedsExpansion.Should().Be(true);
        }
    }
}
