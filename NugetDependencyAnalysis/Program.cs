using System;
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

            var packagesConfigFinder = new PackagesConfigFinder(logger);
            var packagesConfigs = packagesConfigFinder.Find(root);


        }
    }
}
