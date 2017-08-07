using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NugetDependencyAnalysis.Finding;
using NugetDependencyAnalysis.Parsing.FileReading;
using Serilog;

namespace NugetDependencyAnalysis.Parsing
{
    internal class PackagesConfigParser
    {
        public PackagesConfigParser(ILogger logger, IFileReader fileReader)
        {
            Logger = logger;
            FileReader = fileReader;
        }

        private IFileReader FileReader { get; }

        private ILogger Logger { get; }

        public ProjectNugetsGrouping Parse(PackagesConfigFile packagesConfigFile)
        {
            var contents = FileReader.ReadFileContents(packagesConfigFile.Path);
            if (contents == string.Empty)
            {
                Logger.Warning("Config file located at {Path} is empty", packagesConfigFile.Path);
                return EmptyProjectNugetsGrouping(packagesConfigFile.ProjectName);
            }

            XDocument xDocument;
            try
            {
                xDocument = XDocument.Parse(contents);
            }
            catch (XmlException e)
            {
                Logger.Warning("Config file located at {Path} contains invalid XML - {XmlExceptionMessage}", packagesConfigFile.Path, e.Message);
                return EmptyProjectNugetsGrouping(packagesConfigFile.ProjectName);
            }

            try
            {
                var packages = from package in xDocument.Descendants("package")
                               select new NugetPackage(
                                   package.Attribute("id").Value,
                                   package.Attribute("version").Value,
                                   package.Attribute("targetFramework").Value);

                return new ProjectNugetsGrouping(packagesConfigFile.ProjectName, packages.ToList());
            }
            catch (NullReferenceException)
            {
                Logger.Warning("Config file located at {Path} contains package elements with missing id, version, or targetFramework attributes", packagesConfigFile.Path);
                return EmptyProjectNugetsGrouping(packagesConfigFile.ProjectName);
            }
        }

        private ProjectNugetsGrouping EmptyProjectNugetsGrouping(string projectName)
        {
            return new ProjectNugetsGrouping(projectName, new List<NugetPackage>());
        }
    }
}
