using System;
using System.Collections.Generic;
using System.Linq;

namespace NugetDependencyAnalysis.Comparing
{
    internal class TargetFrameworkProjectsGrouping : IEquatable<TargetFrameworkProjectsGrouping>
    {
        public TargetFrameworkProjectsGrouping(string targetFramework, IEnumerable<string> projectNames)
        {
            TargetFramework = targetFramework;
            ProjectNames = new List<string>(projectNames);
        }

        public string TargetFramework { get; }

        public IReadOnlyList<string> ProjectNames { get; }

        public bool Equals(TargetFrameworkProjectsGrouping other)
        {
            return TargetFramework == other.TargetFramework &&
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

            return Equals((TargetFrameworkProjectsGrouping)obj);
        }

        public override int GetHashCode()
        {
            throw new InvalidOperationException($"{nameof(TargetFrameworkProjectsGrouping)} is not intended to be the key in a collection.");
        }

        public override string ToString()
        {
            return TargetFramework;
        }
    }
}
