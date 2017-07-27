using System;

namespace NugetDependencyAnalysis
{
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
