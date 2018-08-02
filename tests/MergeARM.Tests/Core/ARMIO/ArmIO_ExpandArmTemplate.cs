using FluentAssertions;
using MergeARM.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            sut.ExpandArmTemplate(arm);

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

            sut.ExpandArmTemplate(arm);

            arm.ExpandedContent.SelectTokens("$..template").Any().Should().Be(true);
        }
    }
}
