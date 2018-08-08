using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace MergeARM.Tests.Core
{
    public class MockFileSystemImpl
    {
        public static string CurrentDir = @"C:\some\unrelated\dir";

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

                    { @"c:\arm.template.with.templateLink.relative.json", new MockFileData(
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
                                                ""uri"": ""file://.\\reusable.templates\\arm.linked.minimal.template.json"",
                                            ""contentVersion"": ""1.0.0.0""
                                        }
                                    }
                                }
                            ]
                         }") },

                    { @"c:\arm.template.with.forward.slash.templateLink.json", new MockFileData(
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
                                                ""uri"": ""file://C:/reusable.templates/arm.linked.minimal.template.json"",
                                            ""contentVersion"": ""1.0.0.0""
                                        }
                                    }
                                }
                            ]
                         }") },

                    { @"c:\reusable.templates\arm.linked.minimal.template.json", new MockFileData(
                        @"
                         {
                            ""$schema"": ""https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#"",
                            ""contentVersion"": ""1.0.0.0"",
                            ""resources"": [
                              {
                                ""type"": ""Microsoft.Storage/storageAccounts"",
                                ""name"": ""[variables('storageName')]"",
                                ""apiVersion"": ""2015-06-15"",
                                ""location"": ""West US"",
                                ""properties"": {
                                  ""accountType"": ""Standard_LRS""
                                }
                              }
                            ]
                          }")
                    },
                    { @"c:\arm.expected.extended.template.json", new MockFileData(
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
                                        ""template"": {
                                            ""$schema"": ""https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#"",
                                            ""contentVersion"": ""1.0.0.0"",
                                            ""resources"": [
                                              {
                                                ""type"": ""Microsoft.Storage/storageAccounts"",
                                                ""name"": ""[variables('storageName')]"",
                                                ""apiVersion"": ""2015-06-15"",
                                                ""location"": ""West US"",
                                                ""properties"": {
                                                  ""accountType"": ""Standard_LRS""
                                                }
                                              }
                                            ]
                                          }
                                    }
                                }
                            ]
                         }") }
                }, currentDirectory: CurrentDir);
            }
        }
    }
}
