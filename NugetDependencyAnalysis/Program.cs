using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public IReadOnlyList<PackagesConfig> Find(string directory)
        {            
            var packagesConfigs = new List<PackagesConfig>();

            var root = new DirectoryInfo(directory);

            return root.GetFiles("packages.config", SearchOption.AllDirectories)
                .Select(packagesConfigFile => CreatePackagesConfig(packagesConfigFile))
                .ToList();
        }

        private PackagesConfig CreatePackagesConfig(FileInfo packagesConfigFile)
        {
            var projectFiles = packagesConfigFile.Directory.GetFiles("*.csproj", SearchOption.TopDirectoryOnly)
                .ToList();

            // TODO: Better error handling.
            if (projectFiles.Count == 0)
            {
                throw new Exception();
            }
            if (projectFiles.Count > 1)
            {
                throw new Exception();
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
