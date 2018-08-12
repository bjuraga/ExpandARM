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

        public void SaveExpandedTemplate(ArmTemplate armTemplate)
        {
            var storeFileName = fileSystem.Path.ChangeExtension(armTemplate.FilePath, "expanded.josn");

            armTemplate.ExpandedFileName = storeFileName;

            fileSystem.File.WriteAllText(storeFileName, armTemplate.ExpandedContent.ToString());
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
                    string templateFullPath = GetFullPath(armTemplate.FilePath, templateFilePath);

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
                fileSystem.Path.Combine(fileSystem.Path.GetDirectoryName(hostFilePath), templateFilePath);

            var fullPath = fileSystem.Path.GetFullPath(path);

            return fullPath;
        }
    }
}
