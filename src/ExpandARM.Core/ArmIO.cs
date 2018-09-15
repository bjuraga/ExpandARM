using ExpandARM.Core.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace ExpandARM.Core
{
    public class ArmIO : IArmIO
    {
        private readonly IFileSystem fileSystem;

        private ArmIO(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public ArmTemplate LoadArmTemplate(string filePath)
        {
            var fullPath = GetFullPath(fileSystem.Directory.GetCurrentDirectory(), filePath);

            var fileText = fileSystem.File.ReadAllText(fullPath);
            var jObject = JObject.Parse(fileText);
            return new ArmTemplate(fullPath, jObject);
        }

        public void ExpandArmTemplate(ArmTemplate armTemplate)
        {
            HashSet<string> uniqueArmTemplateFullPaths = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            ExpandArmTemplateImpl(armTemplate, uniqueArmTemplateFullPaths);
        }

        public string SaveExpandedTemplate(ArmTemplate armTemplate)
        {
            return SaveExpandedTemplate(armTemplate, null);
        }

        public string SaveExpandedTemplate(ArmTemplate armTemplate, string outputFilePath)
        {
            var acctualOutputFilePath =
                string.IsNullOrWhiteSpace(outputFilePath) ?
                fileSystem.Path.ChangeExtension(armTemplate.FilePath, "expanded.json") :
                outputFilePath;

            fileSystem.File.WriteAllText(acctualOutputFilePath, armTemplate.ExpandedContent.ToString());

            return acctualOutputFilePath;
        }

        public static IArmIO Create(IFileSystem fileSystem)
        {
            return new ArmIO(fileSystem);
        }

        private void ExpandArmTemplateImpl(ArmTemplate armTemplate, HashSet<string> uniqueArmTemplateFullPaths)
        {
            armTemplate
                .ExpandedContent
                .SelectTokens("$..templateLink")
                .ToList()
                .ForEach(t =>
                {
                    var templateFilePath = ((string)((dynamic)t).uri).Replace("file://", "").Replace("/", "\\");
                    string templateFullPath = GetFullPath(fileSystem.Path.GetDirectoryName(armTemplate.FilePath), templateFilePath);

                    if (string.Equals(armTemplate.FilePath, templateFullPath, StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new SelfReferenceException("This template contains a link to self which will cause a neverending expansion loop.");
                    }

                    if (uniqueArmTemplateFullPaths.Contains(templateFullPath))
                    {
                        throw new ReferenceLoopException($"This template contains a link to one of its parents. File: {templateFullPath}");
                    }

                    uniqueArmTemplateFullPaths.Add(templateFullPath);

                    var nestedTemplate = LoadArmTemplate(templateFullPath);

                    ExpandArmTemplateImpl(nestedTemplate, uniqueArmTemplateFullPaths);

                    t.Parent.Parent["template"] = nestedTemplate.ExpandedContent;

                    ((JObject)t.Parent.Parent).Property("templateLink").Remove();
                });
        }

        private string GetFullPath(string hostFilePath, string templateFilePath)
        {
            var path = fileSystem.Path.IsPathRooted(templateFilePath) ?
                templateFilePath :
                fileSystem.Path.Combine(hostFilePath, templateFilePath);

            var fullPath = fileSystem.Path.GetFullPath(path);

            return fullPath;
        }
    }
}
