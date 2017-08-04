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

        public IReadOnlyList<PackagesConfigFile> Find(string directory)
        {
            var packagesConfigFiles = new List<PackagesConfigFile>();

            var root = new DirectoryInfo(directory);
            if (!root.Exists)
            {
                Logger.Error("{Directory} does not exist", directory);
                return packagesConfigFiles;
            }

            var matchingFiles = root.GetFiles(PackagesConfigFileName, SearchOption.AllDirectories)
                .ToList();

            foreach (var file in matchingFiles)
            {
                var packagesConfig = CreatePackagesConfig(file);
                if (packagesConfig == null)
                {
                    continue;
                }

                packagesConfigFiles.Add(packagesConfig);
            }

            return packagesConfigFiles;
        }

        private PackagesConfigFile CreatePackagesConfig(FileInfo file)
        {
            var projectFiles = file.Directory.GetFiles("*.csproj", SearchOption.TopDirectoryOnly)
                .ToList();

            if (projectFiles.Count == 0)
            {
                Logger.Warning($"Skipping {PackagesConfigFileName} in {{Directory}} because the project file is missing", file.Directory.FullName);
                return null;
            }
            if (projectFiles.Count > 1)
            {
                Logger.Warning($"Skipping {PackagesConfigFileName} in {{Directory}} because there are multiple project files", file.Directory.FullName);
                return null;
            }

            var projectFile = projectFiles.Single();
            var projectName = projectFile.Name.Substring(0, projectFile.Name.Length - projectFile.Extension.Length);

            return new PackagesConfigFile(file.FullName, projectName);
        }
    }
}
