using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;

namespace NugetDependencyAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    internal class PackagesConfigFinder
    {
        private const string PackagesConfigFileName = "packages.config";

        public PackagesConfigFinder(ILogger logger)
        {
            Logger = logger;
        }

        private ILogger Logger { get; }

        public IReadOnlyList<PackagesConfig> Find(string directory)
        {            
            var packagesConfigs = new List<PackagesConfig>();

            var root = new DirectoryInfo(directory);

            var packagesConfigFiles = root.GetFiles(PackagesConfigFileName, SearchOption.AllDirectories)
                .ToList();

            foreach (var packagesConfigFile in packagesConfigFiles)
            {
                var packagesConfig = CreatePackagesConfig(packagesConfigFile);
                if (packagesConfig == null)
                {
                    continue;
                }

                packagesConfigs.Add(packagesConfig);
            }

            return packagesConfigs;
        }

        private PackagesConfig CreatePackagesConfig(FileInfo packagesConfigFile)
        {
            var projectFiles = packagesConfigFile.Directory.GetFiles("*.csproj", SearchOption.TopDirectoryOnly)
                .ToList();
            
            if (projectFiles.Count == 0)
            {
                Logger.Warning($"Skipping {PackagesConfigFileName} in {{Directory}} because the project file is missing", packagesConfigFile.Directory.FullName);
                return null;
            }
            if (projectFiles.Count > 1)
            {
                Logger.Warning($"Skipping {PackagesConfigFileName} in {{Directory}} because there are multiple project files", packagesConfigFile.Directory.FullName);
                return null;
            }

            var projectFile = projectFiles.Single();
            var projectName = projectFile.Name.Substring(0, projectFile.Name.Length - projectFile.Extension.Length);

            return new PackagesConfig(packagesConfigFile.FullName, projectName);
        }
    }

    internal class PackagesConfig
    {
        public PackagesConfig(string path, string projectName)
        {
            Path = path;
            ProjectName = projectName;
        }

        public string Path { get; }

        public string ProjectName { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) || obj.GetType() != GetType())
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals((PackagesConfig)obj);
        }

        protected bool Equals(PackagesConfig other)
        {
            return Path == other.Path &&
                ProjectName == other.ProjectName;
        }

        public override int GetHashCode()
        {
            throw new InvalidOperationException($"{nameof(PackagesConfig)} is not intended to be the key in a collection.");
        }
    }
}
