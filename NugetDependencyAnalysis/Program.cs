using System;
using System.Linq;
using NugetDependencyAnalysis.Comparing;
using NugetDependencyAnalysis.Finding;
using NugetDependencyAnalysis.Parsing;
using NugetDependencyAnalysis.Parsing.FileReading;
using Serilog;

namespace NugetDependencyAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("NugetDependencyAnalysis expects a single parameter, the directory to analyse in.");
                Console.WriteLine();
                Console.WriteLine("Usage: NugetDependencyAnalysis.exe <directory to analyse>");
                Console.WriteLine("  E.g. NugetDependencyAnalysis.exe \"C:\\repos\"");
                Console.WriteLine();
                return;
            }

            var root = args[0];

            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            var finder = new PackagesConfigFinder(logger);
            var parser = new PackagesConfigParser(logger, new FileReader());
            var comparer = new ProjectsNugetsComparer();

            var packagesConfigFiles = finder.Find(root);
            var projectsNugets = packagesConfigFiles.Select(packagesConfigFile => parser.Parse(packagesConfigFile));
            var nugetDifferences = comparer.Compare(projectsNugets);

            foreach (var difference in nugetDifferences)
            {
                if (difference.TargetFrameworkDifferences.Count > 0)
                {
                    logger.Information("{Package} has different frameworks targetted {TargetFrameworks} across different projects"
                        , difference.PackageName
                        , difference.TargetFrameworkDifferences.Select(targetFrameworkDifference => targetFrameworkDifference.TargetFramework));

                    foreach (var targetFrameworkDifference in difference.TargetFrameworkDifferences)
                    {
                        logger.Information("{Package} {TargetFramework} referenced in projects {Projects}",
                            difference.PackageName,
                            targetFrameworkDifference.TargetFramework,
                            targetFrameworkDifference.ProjectNames);
                    }
                }

                if (difference.VersionDifferences.Count > 0)
                {
                    logger.Information("{Package} has multiple versions {Versions} referenced in different projects"
                        , difference.PackageName
                        , difference.VersionDifferences.Select(versionDifference => versionDifference.Version));

                    foreach (var versionDifference in difference.VersionDifferences)
                    {
                        logger.Information("{Package} {Version} referenced in projects {Projects}",
                            difference.PackageName,
                            versionDifference.Version,
                            versionDifference.ProjectNames);
                    }
                }
            }
        }
    }
}
