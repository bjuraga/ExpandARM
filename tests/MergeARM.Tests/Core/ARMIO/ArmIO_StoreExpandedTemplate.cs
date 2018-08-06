using FluentAssertions;
using MergeARM.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace MergeARM.Tests.Core.ARMIO
{
    [TestClass]
    public class ArmIO_StoreExpandedTemplate
    {
        [TestMethod]
        public void SaveExpandedTemplate_Should_Be_As_Expected_Template()
        {
            // Arrange
            var fileSystem = MockFileSystemImpl.FileSystem;
            var filePath = @"c:\arm.template.with.templateLink.json";
            var sut = ArmIO.Create(fileSystem);
            var arm = sut.LoadArmTemplate(filePath);
            sut.ExpandArmTemplate(arm);

            // Act
            sut.SaveExpandedTemplate(arm);

            // Assert
            JObject contentInExpandedFile = JObject.Parse(fileSystem.File.ReadAllText(arm.ExpandedFileName));
            JObject expectedContent = JObject.Parse(fileSystem.File.ReadAllText(@"c:\arm.expected.extended.template.json"));
            contentInExpandedFile.Should().BeEquivalentTo(expectedContent);
        }
    }
}
