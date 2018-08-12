using ExpandARM.Core;
using ExpandARM.Core.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.IO.Abstractions;
using System.Linq;

namespace ExpandARM.Tests.Core.ARMIO
{
    [TestClass]
    public class ArmIO_ExpandArmTemplate
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
        public void ExpandArmTemplate_Sets_IsExpanded_To_True()
        {
            // Arrange
            var filePath = @"c:\templates\main\arm.template.with.templateLink.json";
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
            var filePath = @"c:\templates\main\arm.template.with.templateLink.json";
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
            var filePath = @"c:\templates\main\arm.template.with.templateLink.json";
            var expectedTemplateContent = JObject.Parse(fileSystem.File.ReadAllText(@"c:\templates\reusable\arm.linked.minimal.template.json"));
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
            var filePath = @"c:\templates\main\arm.template.with.forward.slash.templateLink.json";
            var expectedTemplateContent = JObject.Parse(fileSystem.File.ReadAllText(@"c:\templates\reusable\arm.linked.minimal.template.json"));
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            sut.ExpandArmTemplate(arm);

            // Assert
            arm.ExpandedContent.SelectToken("$..template").ToString().Should().BeEquivalentTo(expectedTemplateContent.ToString());
        }

        [TestMethod]
        public void ExpandArmTemplate_Template_Contains_FileContents_Of_LinkedTemplateWithRelativePath()
        {
            // Arrange
            var filePath = @"c:\templates\main\arm.template.with.templateLink.relative.json";
            var expectedTemplateContent = JObject.Parse(fileSystem.File.ReadAllText(@"c:\templates\reusable\arm.linked.minimal.template.json"));
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            sut.ExpandArmTemplate(arm);

            // Assert
            arm.ExpandedContent.SelectToken("$..template").ToString().Should().BeEquivalentTo(expectedTemplateContent.ToString());
        }

        [TestMethod]
        public void ExpandArmTemplate_ContainingNestedTemplate_ContainsFileContents_Of_AllNestedTemplates()
        {
            // Arrange
            var filePath = @"c:\templates\main\minimal.arm.template.nested.3.levels.json";
            var expectedTemplateContent = JObject.Parse(fileSystem.File.ReadAllText(@"c:\templates\main\minimal.arm.template.nested.3.levels.test.json"));
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            sut.ExpandArmTemplate(arm);

            // Assert
            arm.ExpandedContent.ToString().Should().BeEquivalentTo(expectedTemplateContent.ToString());
        }

        [TestMethod]
        public void ExpandArmTemplate_ReferencingSelf_Throws_SelfReferenceException()
        {
            // Arrange
            var filePath = @"c:\templates\main\minimal.arm.template.self.referencing.json";
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            Action act = () => sut.ExpandArmTemplate(arm);

            // Assert
            act.Should().Throw<SelfReferenceException>();
        }

        [TestMethod]
        public void ExpandArmTemplate_ReferencingParent_Throws_ReferenceLoopException()
        {
            // Arrange
            var filePath = @"c:\templates\main\minimal.arm.template.selfreference.parent.json";
            var arm = sut.LoadArmTemplate(filePath);

            // Act
            Action act = () => sut.ExpandArmTemplate(arm);

            // Assert
            act.Should().Throw<ReferenceLoopException>();
        }
    }
}
