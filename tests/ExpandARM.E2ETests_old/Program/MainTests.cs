using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;

namespace ExpandARM.E2ETests
{
    [TestClass]
    public class MainTests
    {
        private static string PublishedExeFilePath = "ExpandARM.exe";

        [ClassInitialize]
        public static void BeforeAll(TestContext testContext)
        {
            var rootFolder = GetRepositoryRootDir();
            PublishedExeFilePath = $@"{rootFolder}\src\ExpandARM\bin\Release\ExpandARM.exe"; 
        }

        private static string GetRepositoryRootDir()
        {
            string dir = Directory.GetCurrentDirectory();
            do
            {
                dir = Directory.GetParent(dir).FullName;
            } while (Directory.GetDirectoryRoot(dir) != dir && new DirectoryInfo(dir).GetDirectories(".git", SearchOption.TopDirectoryOnly).Length != 1);

            return dir;
        }

        [TestMethod, TestCategory("e2e")]
        public void Main_WithMinimalTemplateWithoutLinks_Succeeds()
        {
            // Arrange
            var inputFileName = @"TestData\templates\main\minimal.template.without.links.json";
            var outputFileName = @"TestData\templates\main\minimal.template.without.links.expanded.json";
            var argumentsToPass = $"-i {inputFileName}";

            // Act
            Process.Start(PublishedExeFilePath, argumentsToPass).WaitForExit();

            // Assert
            var inputFileJObject = JObject.Parse(File.ReadAllText(inputFileName));

            var outputFileJObject = JObject.Parse(File.ReadAllText(outputFileName));

            inputFileJObject.Should().BeEquivalentTo(outputFileJObject);
        }

        [TestMethod, TestCategory("e2e")]
        public void Main_WithMinimalTemplateWithoutLinks_UsesTheOutputFileParameter()
        {
            // Arrange
            var inputFileName = @"TestData\templates\main\minimal.template.without.links.json";
            var outputFileName = @"TestData\templates\main\minimal.template.without.links.processed.json";
            var argumentsToPass = $"-i {inputFileName} -o {outputFileName}";

            // Act
            Process.Start(PublishedExeFilePath, argumentsToPass).WaitForExit();

            // Assert
            var inputFileJObject = JObject.Parse(File.ReadAllText(inputFileName));
            var outputFileJObject = JObject.Parse(File.ReadAllText(outputFileName));
            inputFileJObject.Should().BeEquivalentTo(outputFileJObject);
        }
    }
}
