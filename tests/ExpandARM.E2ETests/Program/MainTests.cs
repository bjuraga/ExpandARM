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
        private static string PublishedExeFilePath;

        [ClassInitialize]
        public static void BeforeAll(TestContext testContext)
        {
            PublishedExeFilePath = PublishAppAsExe();
        }

        [TestMethod]
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

        [TestMethod]
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

        private static string PublishAppAsExe()
        {
            //var srcDir = @"..\..\..\..\..\src\ExpandARM";

            //var publishDir = Path.Combine(srcDir, @"bin\publish");
            //if (Directory.Exists(publishDir))
            //{
            //    Directory.Delete(publishDir, true);
            //}

            //var argumentsToPass = $@"publish ExpandARM.csproj -c Release -r win-x64 -o bin\publish --self-contained";
            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = "dotnet",
            //    Arguments = argumentsToPass,
            //    WorkingDirectory = srcDir
            //}).WaitForExit();

            Console.WriteLine("tests running in: " + Directory.GetCurrentDirectory());
            var sut = Directory.GetFiles(@".\..\..\..\..\..\e2e", "ExpandARM.exe", SearchOption.AllDirectories);

            if (sut.Length == 0)
            {
                Assert.Fail("ExpandARM.exe not found in publish dir");
            }

            return sut[0];
        }
    }
}
