using System;

namespace NugetDependencyAnalysis.Finding
{
    internal class PackagesConfigFile
    {
        public PackagesConfigFile(string path, string projectName)
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

            return Equals((PackagesConfigFile)obj);
        }

        protected bool Equals(PackagesConfigFile other)
        {
            return Path == other.Path &&
                ProjectName == other.ProjectName;
        }

        public override int GetHashCode()
        {
            throw new InvalidOperationException($"{nameof(PackagesConfigFile)} is not intended to be the key in a collection.");
        }
    }
}
