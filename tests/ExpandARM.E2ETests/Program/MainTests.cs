using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;

namespace ExpandARM.E2ETests
{
    [TestClass]
    public class MainTests
    {
        [TestMethod]
        public void Main_WithMinimalTemplateWithoutLinks_Succeeds()
        {
            // Arrange
            var inputFileName = @"TestData\templates\main\minimal.template.without.links.json";
            var outputFileName = @"TestData\templates\main\minimal.template.without.links.expanded.json";
            var argumentsToPass = $"-i {inputFileName}";

            // Act
            Process.Start("ExpandARM.exe", argumentsToPass).WaitForExit();

            // Assert
            var inputFileJObject = JObject.Parse(File.ReadAllText(inputFileName));
            var outputFileJObject = JObject.Parse(File.ReadAllText(outputFileName));
            inputFileJObject.Should().BeEquivalentTo(outputFileJObject);
        }
    }
}
