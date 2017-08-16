using System;
using System.Collections.Generic;
using System.Linq;

namespace NugetDependencyAnalysis.Comparing
{
    internal class VersionProjectsGrouping : IEquatable<VersionProjectsGrouping>
    {
        public VersionProjectsGrouping(string version, IReadOnlyList<string> projectNames)
        {
            Version = version;
            ProjectNames = projectNames;
        }

        public string Version { get; }

        public IReadOnlyList<string> ProjectNames { get; }

        public bool Equals(VersionProjectsGrouping other)
        {
            return Version == other.Version &&
                ProjectNames.SequenceEqual(other.ProjectNames);
        }

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

            return Equals((VersionProjectsGrouping)obj);
        }

        public override int GetHashCode()
        {
            throw new InvalidOperationException($"{nameof(VersionProjectsGrouping)} is not intended to be the key in a collection.");
        }
    }
}
