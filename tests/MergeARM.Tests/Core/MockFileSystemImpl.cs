﻿using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace MergeARM.Tests.Core
{
    public class MockFileSystemImpl
    {
        public static IFileSystem FileSystem
        {
            get
            {
                return new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { @"c:\minimal.arm.template.json",  new MockFileData("{ \"resources\": []}") },

                    { @"c:\arm.template.with.templateLink.json", new MockFileData(
                        @"{
                            ""$schema"": ""https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#"",
                            ""contentVersion"": ""1.0.0.0"",
                            ""parameters"": {},
                            ""variables"": {},
                            ""resources"": [
                                {
                                    ""apiVersion"": ""2017-05-10"",
                                    ""name"": ""linkedTemplate"",
                                    ""type"": ""Microsoft.Resources/deployments"",
                                    ""properties"": {
                                            ""mode"": ""incremental"",
                                        ""templateLink"": {
                                                ""uri"": ""file://C:\\reusable.templates\\arm.linked.minimal.template.json"",
                                            ""contentVersion"": ""1.0.0.0""
                                        }
                                    }
                                }
                            ]
                         }") },

                    { @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
                });
            }
        }
    }
}
