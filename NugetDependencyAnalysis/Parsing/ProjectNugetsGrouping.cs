using System.Collections.Generic;

namespace NugetDependencyAnalysis.Parsing
{
    public class ProjectNugetsGrouping
    {
        public ProjectNugetsGrouping(string projectName, IReadOnlyList<NugetPackage> nugets)
        {
            ProjectName = projectName;
            Nugets = nugets;
        }

        public string ProjectName { get; }

        public IReadOnlyList<NugetPackage> Nugets { get; }
    }
}
