using System;
using System.Collections.Generic;
using System.Linq;
using NugetDependencyAnalysis.Parsing;

namespace NugetDependencyAnalysis.Comparing
{
    internal class ProjectsNugetsComparer
    {
        public IReadOnlyList<NugetDifferences> Compare(IReadOnlyList<ProjectNugetsGrouping> projects)
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

                var versionDifferences = nugetsGrouping.GroupBy(nugetAndProject => nugetAndProject.nuget.Version)
                    .ToList();

                if (versionDifferences.Count > 0)
                {
                    differences.Add(new NugetDifferences(nugetsGrouping.Key, versionDifferences.Select(item => new VersionProjectsGrouping(item.Key, item.Select(i => i.projectName).ToList())).ToList()));
                }
            }

            return differences;
        }
    }
}
