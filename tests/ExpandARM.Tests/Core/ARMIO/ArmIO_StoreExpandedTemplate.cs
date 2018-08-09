﻿using ExpandARM.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.IO.Abstractions;

namespace ExpandARM.Tests.Core.ARMIO
{
    [TestClass]
    public class ArmIO_StoreExpandedTemplate
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
        public void SaveExpandedTemplate_Should_Be_As_Expected_Template()
        {
            // Arrange
            var filePath = @"c:\templates\main\arm.template.with.templateLink.json";
            var arm = sut.LoadArmTemplate(filePath);
            sut.ExpandArmTemplate(arm);

            // Act
            sut.SaveExpandedTemplate(arm);

            // Assert
            JObject contentInExpandedFile = JObject.Parse(fileSystem.File.ReadAllText(arm.ExpandedFileName));
            JObject expectedContent = JObject.Parse(fileSystem.File.ReadAllText(@"c:\templates\main\arm.expected.extended.template.json"));
            contentInExpandedFile.Should().BeEquivalentTo(expectedContent);
        }
    }
}