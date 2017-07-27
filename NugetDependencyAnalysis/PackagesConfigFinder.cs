using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;

namespace NugetDependencyAnalysis
{
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
            if (!root.Exists)
            {
                Logger.Error("{Directory} does not exist", directory);
                return packagesConfigs;
            }

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
}
