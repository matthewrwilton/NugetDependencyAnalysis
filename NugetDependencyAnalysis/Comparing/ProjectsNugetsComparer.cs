using System.Collections.Generic;
using System.Linq;
using NugetDependencyAnalysis.Parsing;

namespace NugetDependencyAnalysis.Comparing
{
    internal class ProjectsNugetsComparer
    {
        public IReadOnlyList<NugetDifferences> Compare(IEnumerable<ProjectNugetsGrouping> projects)
        {
            IEnumerable<(string projectName, NugetPackage nuget)> nugets = projects.SelectMany(project => project.Nugets.Select(nuget => (project.ProjectName, nuget)));

            var nugetsGroupedByPackage = nugets.GroupBy(nugetAndProject => nugetAndProject.nuget.Name);

            var differences = new List<NugetDifferences>();
            foreach (var nugetsGrouping in nugetsGroupedByPackage)
            {
                if (nugetsGrouping.Count() == 1)
                {
                    continue;
                }

                var targetFrameworks = nugetsGrouping.GroupBy(nugetAndProject => nugetAndProject.nuget.TargetFramework)
                        .ToList();
                var targetFrameworkDifferences = new List<TargetFrameworkProjectsGrouping>();
                if (targetFrameworks.Count > 1)
                {
                    targetFrameworkDifferences = targetFrameworks.Select(item => new TargetFrameworkProjectsGrouping(item.Key, item.Select(i => i.projectName)))
                        .ToList();
                }

                var versions = nugetsGrouping.GroupBy(nugetAndProject => nugetAndProject.nuget.Version)
                        .ToList();
                var versionDifferences = new List<VersionProjectsGrouping>();
                if (versions.Count > 1)
                {
                    versionDifferences = versionDifferences = versions.Select(item => new VersionProjectsGrouping(item.Key, item.Select(i => i.projectName)))
                        .ToList();
                }

                if (targetFrameworkDifferences.Count > 0 || versionDifferences.Count > 0)
                {
                    var nugetDifferences = new NugetDifferences(nugetsGrouping.Key, targetFrameworkDifferences, versionDifferences);
                    differences.Add(nugetDifferences);
                }
            }

            return differences;
        }
    }
}
